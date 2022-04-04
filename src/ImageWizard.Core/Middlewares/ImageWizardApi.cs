// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Caches;
using ImageWizard.Core;
using ImageWizard.Core.Locking;
using ImageWizard.Loaders;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

/// <summary>
/// ImageWizardApi
/// </summary>
public class ImageWizardApi
{
    private readonly AsyncLock<string> asyncLock = new AsyncLock<string>();

    /// <summary>
    /// ExecuteAsync
    /// </summary>
    public async Task<IResult> ExecuteAsync(
                                            HttpContext context,
                                            IOptions<ImageWizardOptions> options,
                                            ILogger<ImageWizardApi> logger,
                                            IUrlSignature signatureService,
                                            ICache cache,
                                            ICacheKey cacheKey,
                                            ICacheHash cacheHash,
                                            InterceptorInvoker interceptor,
                                            ImageWizardBuilder builder,
                                            string signature,
                                            string path)
    {
        if (context.Request.QueryString.HasValue)
        {
            path += context.Request.QueryString.Value;
        }

        //parse url
        if (ImageWizardUrl.TryParse(path, out ImageWizardUrl url) == false)
        {
            return Results.Problem(detail: "Url is invalid.", statusCode: StatusCodes.Status400BadRequest);
        }

        //unsafe url?
        if (signature == ImageWizardDefaults.Unsafe)
        {
            if (options.Value.AllowUnsafeUrl)
            {
                logger.LogTrace("Unsafe request");

                interceptor.OnUnsafeSignature();
            }
            else
            {
                Results.Problem(detail: "Unsafe url is not allowed!", statusCode: StatusCodes.Status403Forbidden);
            }
        }
        else
        {
            //check signature
            string validSignature = signatureService.Encrypt(options.Value.KeyInBytes, new ImageWizardRequest(url, context.Request.Host));

            if (signature == validSignature)
            {
                logger.LogTrace("Signature is valid");

                interceptor.OnValidSignature();
            }
            else
            {
                interceptor.OnInvalidSignature();

                return Results.Problem(detail: "Signature is not valid!", statusCode: StatusCodes.Status403Forbidden);
            }
        }

        StringBuilder pathBuilder = new StringBuilder(url.Path);

        //get compatible mime types by the accept header
        IEnumerable<string> acceptMimeTypes = context.Request.GetTypedHeaders().Accept
                                                                    .Where(x => x.MatchesAllSubTypes == false)
                                                                    .Select(x => x.MediaType.Value)
                                                                    //filter unknown mime types
                                                                    .Where(x => builder.PipelineManager.ContainsKey(x))
                                                                    .ToList();

        //use accept header?
        if (options.Value.UseAcceptHeader)
        {
            if (acceptMimeTypes.Any())
            {
                pathBuilder.Append($"+Accept={string.Join(",", acceptMimeTypes)}");
            }
        }

        ClientHints clientHints = new ClientHints(options.Value.AllowedDPR);

        //use client hints?
        if (options.Value.UseClientHints)
        {
            clientHints = context.Request.GetClientHints(options.Value.AllowedDPR);

            if (clientHints.DPR != null)
            {
                pathBuilder.Append($"+CH_DPR={clientHints.DPR}");
            }

            if (clientHints.Width != null)
            {
                pathBuilder.Append($"+CH_Width={clientHints.Width}");
            }

            if (clientHints.ViewportWidth != null)
            {
                pathBuilder.Append($"+CH_ViewportWidth={clientHints.ViewportWidth}");
            }
        }

        //get data loader
        Type loaderType = builder.LoaderManager.Get(url.LoaderType);
        ILoader? loader = (ILoader?)context.RequestServices.GetService(loaderType);

        if (loader == null)
        {
            return Results.Problem(detail: $"Data loader not found: {url.LoaderType}", statusCode: StatusCodes.Status500InternalServerError);
        }

        //generate data key
        string key = cacheKey.Create(pathBuilder.ToString());

        //create reader lock
        using AsyncLockContext lockState = await asyncLock.ReaderLockAsync(key);

        //read cached data
        ICachedData? cachedData = await cache.ReadAsync(key);

        bool createCachedData = true;

        //cached data found?
        if (cachedData != null)
        {
            createCachedData = loader.Options.Value.RefreshMode switch
            {
                LoaderRefreshMode.None => false,
                LoaderRefreshMode.EveryTime => true,
                LoaderRefreshMode.BasedOnCacheControl => cachedData.Metadata.Cache.NoStore == true
                                                                || cachedData.Metadata.Cache.NoCache == true
                                                                || (cachedData.Metadata.Cache.Expires != null && cachedData.Metadata.Cache.Expires < DateTime.UtcNow),
                _ => throw new Exception($"Unknown refresh mode: {loader.Options.Value.RefreshMode}")
            };
        }

        //create cached data
        if (createCachedData == true)
        {
            logger.LogTrace("Create cached data: {key}", key);

            await lockState.SwitchToWriterLockAsync();

            //read cached data again
            ICachedData? cachedDataAfterWriteLock = await cache.ReadAsync(key);

            //cached data hasn't changed after writer lock?
            if (cachedData?.Metadata.Hash == cachedDataAfterWriteLock?.Metadata.Hash)
            {
                //get original image
                using OriginalData? originalData = await loader.GetAsync(url.LoaderSource, cachedData);

                if (originalData != null) //is there a new version of original image?
                {
                    using PipelineContext processingContext = new PipelineContext(
                                                                     context.RequestServices,
                                                                     context.RequestServices.GetRequiredService<IStreamPool>(),
                                                                     new DataResult(originalData.Data, originalData.MimeType),
                                                                     clientHints,
                                                                     options.Value,
                                                                     acceptMimeTypes,
                                                                     url.Filters);

                    do
                    {
                        //find processing pipeline by mime type
                        Type processingPipelineType = builder.GetPipeline(processingContext.Result.MimeType);
                        IPipeline? processingPipeline = (IPipeline?)context.RequestServices.GetService(processingPipelineType);

                        if (processingPipeline == null)
                        {
                            return Results.Problem(detail: $"Processing pipeline was not found: {processingContext.Result.MimeType}", statusCode: StatusCodes.Status500InternalServerError);
                        }

                        logger.LogTrace("Start pipline: {pipeline}", processingPipelineType.Name);

                        //start processing
                        processingContext.Result = await processingPipeline.StartAsync(processingContext);

                    } while (processingContext.UrlFilters.Count > 0);

                    logger.LogTrace("Save new cached data: {key}", key);

                    processingContext.Result.Data.Seek(0, SeekOrigin.Begin);

                    //create hash of cached image
                    string hash = await cacheHash.CreateAsync(processingContext.Result.Data);

                    DateTime now = DateTime.UtcNow;

                    //create metadata
                    Metadata metadata = new Metadata()
                    {
                        Cache = originalData.Cache,
                        Created = now,
                        LastAccess = now,
                        Key = key,
                        Hash = hash,
                        Filters = url.Filters,
                        LoaderSource = url.LoaderSource,
                        LoaderType = url.LoaderType,
                        MimeType = processingContext.Result.MimeType,
                        FileLength = processingContext.Result.Data.Length
                    };

                    //image result?
                    if (processingContext.Result is ImageResult imageResult)
                    {
                        metadata.Width = imageResult.Width;
                        metadata.Height = imageResult.Height;
                    }

                    processingContext.Result.Data.Seek(0, SeekOrigin.Begin);

                    //write cached data
                    await cache.WriteAsync(metadata, processingContext.Result.Data);

                    //read cached data
                    cachedData = await cache.ReadAsync(key);

                    if (cachedData != null)
                    {
                        interceptor.OnCachedDataCreated(cachedData);
                    }
                }
            }

            await lockState.SwitchToReaderLockAsync(true);
        }

        if (cachedData == null)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }

        //refresh last-access time
        if (options.Value.RefreshLastAccessInterval != null)
        {
            if (cache is ILastAccessCache lastAccessCache)
            {
                if (cachedData.Metadata.LastAccess.Add(options.Value.RefreshLastAccessInterval.Value) < DateTime.UtcNow)
                {
                    await lockState.SwitchToWriterLockAsync();

                    await lastAccessCache.SetLastAccessAsync(key, DateTime.UtcNow);

                    await lockState.SwitchToReaderLockAsync();
                }
            }
            else
            {
                logger.LogWarning("Cache doesn't support last-access refresh.");
            }
        }

        EntityTagHeaderValue etag = new EntityTagHeaderValue($"\"{cachedData.Metadata.Hash}\"");

        //send "304-NotModified" if etag is valid
        if (options.Value.UseETag)
        {
            bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == etag.Tag);

            if (isValid == true)
            {
                logger.LogTrace("Operation completed: 304 Not modified");

                interceptor.OnCachedDataSending(context.Response, cachedData, true);

                return Results.StatusCode(StatusCodes.Status304NotModified);
            }
        }

        ResponseHeaders responseHeaders = context.Response.GetTypedHeaders();

        //use accept header
        if (options.Value.UseAcceptHeader)
        {
            responseHeaders.Append(HeaderNames.Vary, HeaderNames.Accept);
        }

        //use client hints
        if (options.Value.UseClientHints)
        {
            responseHeaders.AppendList(
                                        HeaderNames.Vary,
                                        new[] {
                                            ClientHints.DPRHeader,
                                            ClientHints.WidthHeader,
                                            ClientHints.ViewportWidthHeader
                                        });

            //is image?
            if (cachedData.Metadata.Width != null)
            {
                double? dpr = clientHints.DPR;

                if (clientHints.DPR != null && clientHints.Width != null)
                {
                    dpr = (double)cachedData.Metadata.Width.Value / clientHints.Width.Value / clientHints.DPR.Value;
                }

                if (dpr != null)
                {
                    responseHeaders.Append("Content-DPR", dpr.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        //set cache control header
        if (options.Value.CacheControl.IsEnabled)
        {
            responseHeaders.CacheControl = new CacheControlHeaderValue()
            {
                Public = options.Value.CacheControl.Public,
                MustRevalidate = options.Value.CacheControl.MustRevalidate,
                MaxAge = options.Value.CacheControl.MaxAge,
                NoCache = options.Value.CacheControl.NoCache,
                NoStore = options.Value.CacheControl.NoStore
            };
        }

        //set ETag header
        if (options.Value.UseETag)
        {
            responseHeaders.ETag = etag;
        }

        //is HEAD request?
        bool isHeadRequst = HttpMethods.IsHead(context.Request.Method);

        if (isHeadRequst == true)
        {
            return Results.Ok();
        }

        interceptor.OnCachedDataSending(context.Response, cachedData, false);

        logger.LogTrace("Sending cached data.");

        //send response stream
        Stream stream = await cachedData.OpenReadAsync();

        return Results.File(stream, cachedData.Metadata.MimeType, enableRangeProcessing: true);
    }
}