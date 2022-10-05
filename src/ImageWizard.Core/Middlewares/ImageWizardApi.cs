// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Caches;
using ImageWizard.Loaders;
using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.Text;

namespace ImageWizard;

/// <summary>
/// ImageWizardApi
/// </summary>
public class ImageWizardApi
{
    /// <summary>
    /// ExecuteAsync
    /// </summary>
    public async Task ExecuteAsync(
                                    HttpContext context,
                                    IOptions<ImageWizardOptions> options,
                                    ILogger<ImageWizardApi> logger,
                                    IUrlSignature signatureService,
                                    ICache cache,
                                    ICacheKey cacheKey,
                                    ICacheHash cacheHash,
                                    ICacheLock cacheLock,
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
            logger.LogError("Invalid url: {Url}", path);

            interceptor.OnInvalidUrl(path);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("The url is invalid.");

            return;
        }

        //unsafe url?
        if (signature == ImageWizardDefaults.Unsafe)
        {
            if (options.Value.AllowUnsafeUrl)
            {
                logger.LogTrace("Unsafe request");

                interceptor.OnValidSignature(url);
            }
            else
            {
                logger.LogError("Unsafe url is not allowed!");

                interceptor.OnInvalidSignature(url);

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Unsafe url is not allowed!");

                return;
            }
        }
        else
        {
            //check signature
            string validSignature = signatureService.Encrypt(options.Value.Key, new ImageWizardRequest(url, context.Request.Host));

            if (signature == validSignature)
            {
                logger.LogTrace("Signature is valid.");

                interceptor.OnValidSignature(url);
            }
            else
            {
                logger.LogError("Signature is invalid.");

                interceptor.OnInvalidSignature(url);

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Signature is not valid!");

                return;
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
            logger.LogTrace("Data loader not found: {LoaderType}", url.LoaderType);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync($"Data loader not found: {url.LoaderType}");

            return;
        }

        //generate data key
        string key = cacheKey.Create(pathBuilder.ToString());

        bool createCachedData = true;
        ICachedData? cachedData;

        //create reader lock
        using (await cacheLock.ReaderLockAsync(key))
        {
            //read cached data
            cachedData = await cache.ReadAsync(key);

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
        }

        //create cached data
        if (createCachedData == true)
        {
            logger.LogTrace("Create cached data: {key}", key);

            using var w = await cacheLock.WriterLockAsync(key);

            //read cached data again
            ICachedData? cachedDataAfterWriteLock = await cache.ReadAsync(key);

            //cached data hasn't changed after writer lock?
            if (cachedDataAfterWriteLock == null || cachedData?.Metadata.Hash == cachedDataAfterWriteLock.Metadata.Hash)
            {
                //use the latest cached data
                cachedData = cachedDataAfterWriteLock;

                //get original image
                LoaderResult loaderResult = await loader.GetAsync(url.LoaderSource, cachedData);

                switch (loaderResult.State)
                {
                    case LoaderResultState.NotModified:
                        //ignore
                        break;

                    case LoaderResultState.NotFound:
                    case LoaderResultState.Failed:

                        if (options.Value.FallbackHandler == null)
                        {
                            logger.LogError("Could not load original data and there is no fallback handler registered.");

                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            return;
                        }

                        cachedData = options.Value.FallbackHandler(loaderResult.State, url, cachedData);

                        if (cachedData == null)
                        {
                            logger.LogError("Could not load original data and the fallback handler delivered no cached data.");

                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            return;
                        }
                        break;

                    case LoaderResultState.Success:
                        {
                            using OriginalData? originalData = loaderResult.Result;

                            if (originalData == null)
                            {
                                throw new ArgumentNullException(nameof(originalData));
                            }

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
                                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                    await context.Response.WriteAsync($"Processing pipeline was not found: {processingContext.Result.MimeType}");

                                    return;
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
                        break;
                }
            }
        }

        if (cachedData == null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return;
        }

        //refresh last-access time
        if (options.Value.RefreshLastAccessInterval != null)
        {
            if (cache is ILastAccessCache lastAccessCache)
            {
                if (cachedData.Metadata.LastAccess.Add(options.Value.RefreshLastAccessInterval.Value) < DateTime.UtcNow)
                {
                    using var r1 = await cacheLock.WriterLockAsync(key);

                    await lastAccessCache.SetLastAccessAsync(key, DateTime.UtcNow);
                }
            }
            else
            {
                logger.LogWarning("Cache doesn't support last-access refresh.");
            }
        }

        //set reader lock
        using var r2 = await cacheLock.ReaderLockAsync(key);

        EntityTagHeaderValue etag = new EntityTagHeaderValue($"\"{cachedData.Metadata.Hash}\"");

        //send "304-NotModified" if etag is valid
        if (options.Value.UseETag)
        {
            bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == etag.Tag);

            if (isValid == true)
            {
                logger.LogTrace("Operation completed: 304 Not modified");

                interceptor.OnCachedDataSent(cachedData, true);

                context.Response.StatusCode = StatusCodes.Status304NotModified;

                return;
            }
        }

        ResponseHeaders responseHeaders = context.Response.GetTypedHeaders();
        responseHeaders.ContentType = new MediaTypeHeaderValue(cachedData.Metadata.MimeType);
        responseHeaders.ContentLength = cachedData.Metadata.FileLength;

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
        if (HttpMethods.IsHead(context.Request.Method))
        {
            return;
        }

        logger.LogTrace("Sending cached data.");

        //send response stream
        using (Stream stream = await cachedData.OpenReadAsync())
        {
            await stream.CopyToAsync(context.Response.Body);
        }

        interceptor.OnCachedDataSent(cachedData, false);

        logger.LogTrace("Operation completed");
    }
}