using ImageWizard.Caches;
using ImageWizard.Core;
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

namespace ImageWizard
{
    /// <summary>
    /// ImageWizardApi
    /// </summary>
    public class ImageWizardApi
    {
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        public async Task<IResult> ExecuteAsync(
                                                HttpContext context,
                                                IOptions<ImageWizardOptions> options,
                                                ILogger<ImageWizardApi> logger,
                                                ICache cache,
                                                IEnumerable<IImageWizardInterceptor> interceptors,
                                                ImageWizardBuilder builder,
                                                string path)
        {
            if (context.Request.QueryString.HasValue)
            {
                path += context.Request.QueryString.Value;
            }

            //parse url
            if (ImageWizardUrl.TryParse(path, out ImageWizardUrl url) == false)
            {
                return Results.Problem(detail: "The url is invalid.", statusCode: StatusCodes.Status400BadRequest);
            }

            //unsafe url?
            if (url.IsUnsafeUrl && options.Value.AllowUnsafeUrl)
            {
                logger.LogTrace("unsafe request");
            }
            else
            {
                //check signature
                if (url.IsSignatureValid(options.Value.Key))
                {
                    logger.LogTrace("signature is valid");
                }
                else
                {
                    interceptors.Foreach(x => x.OnFailedSignature());

                    return Results.Problem(detail: "signature is not valid!", statusCode: StatusCodes.Status403Forbidden);
                }
            }            

            string url_path_with_headers = url.Path;

            //get compatible mime types by the accept header
            IEnumerable<string> acceptMimeTypes = context.Request.GetTypedHeaders().Accept
                                                                        .Where(x => x.MatchesAllSubTypes == false)
                                                                        .Select(x => x.MediaType.Value)
                                                                        //filter unknown mime types
                                                                        .Where(x => builder.GetAllMimeTypes().Contains(x))
                                                                        .ToList();

            //use accept header?
            if (options.Value.UseAcceptHeader)
            {
                if (acceptMimeTypes.Any())
                {
                    url_path_with_headers += $"+Accept={string.Join(",", acceptMimeTypes)}";
                }
            }

            //generate data key
            string key = CachedDataKeyHelper.Create(url_path_with_headers);

            //get data loader
            Type loaderType = builder.LoaderManager.Get(url.LoaderType);
            IDataLoader? loader = (IDataLoader?)context.RequestServices.GetService(loaderType);

            if (loader == null)
            {
                return Results.Problem(detail: $"Data loader not found: {url.LoaderType}", statusCode: StatusCodes.Status500InternalServerError);
            }

            ICachedData? cachedData = await cache.ReadAsync(key);

            bool createCachedData = true;

            //cached data found?
            if (cachedData != null)
            {
                createCachedData = loader.Options.Value.RefreshMode switch
                {
                    DataLoaderRefreshMode.None => false,
                    DataLoaderRefreshMode.EveryTime => true,
                    DataLoaderRefreshMode.BasedOnCacheControl => cachedData.Metadata.Cache.NoStore == true
                                                                    || cachedData.Metadata.Cache.NoCache == true
                                                                    || (cachedData.Metadata.Cache.Expires != null && cachedData.Metadata.Cache.Expires < DateTime.UtcNow),
                    _ => throw new Exception("Unknown refresh mode.")
                };
            }

            //create cached data
            if (createCachedData == true)
            {
                logger.LogTrace("Create cached data");

                //get original image
                using OriginalData? originalData = await loader.GetAsync(url.LoaderSource, cachedData);

                if (originalData != null) //is there a new version of original image?
                {
                    ClientHints clientHints = context.Request.GetClientHints(options.Value.AllowedDPR);

                    using ProcessingPipelineContext processingContext = new ProcessingPipelineContext(
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
                        IProcessingPipeline? processingPipeline = (IProcessingPipeline?)context.RequestServices.GetService(processingPipelineType);

                        if (processingPipeline == null)
                        {
                            return Results.Problem(detail: $"Processing pipeline was not found: {processingContext.Result.MimeType}", statusCode: StatusCodes.Status500InternalServerError);
                        }

                        logger.LogTrace($"Start pipline: {processingPipelineType.Name}");

                        //start processing
                        processingContext.Result = await processingPipeline.StartAsync(processingContext);

                    } while (processingContext.UrlFilters.Count > 0);

                    logger.LogTrace("Save new cached data");

                    using SHA256 sha256 = SHA256.Create();

                    //create hash of cached image
                    byte[] hash = sha256.ComputeHash(processingContext.Result.Data);

                    //create metadata
                    Metadata metadata = new Metadata()
                    {
                        Cache = originalData.Cache,
                        Created = DateTime.UtcNow,
                        Key = key,
                        Filters = url.Filters,
                        LoaderSource = url.LoaderSource,
                        LoaderType = url.LoaderType,
                        Hash = Convert.ToHexString(hash),
                        MimeType = processingContext.Result.MimeType,
                        FileLength = processingContext.Result.Data.Length
                    };

                    //image result?
                    if (processingContext.Result is ImageResult imageResult)
                    {
                        metadata.DPR = imageResult.DPR;
                        metadata.Width = imageResult.Width;
                        metadata.Height = imageResult.Height;
                    }

                    processingContext.Result.Data.Seek(0, SeekOrigin.Begin);

                    //write cached data
                    await cache.WriteAsync(key, metadata, processingContext.Result.Data);
                    
                    //read cached data
                    cachedData = await cache.ReadAsync(key);

                    if (cachedData != null)
                    {
                        interceptors.Foreach(x => x.OnCachedImageCreated(cachedData));
                    }
                }
            }

            if (cachedData == null)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            EntityTagHeaderValue etag = new EntityTagHeaderValue($"\"{cachedData.Metadata.Hash}\"");

            //send "304-NotModified" if etag is valid
            if (options.Value.UseETag)
            {
                bool? isValid = context.Request.GetTypedHeaders().IfNoneMatch?.Any(x => x.Tag == etag.Tag);

                if (isValid == true)
                {
                    logger.LogTrace("Operation completed: 304 Not modified");

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
            if (options.Value.UseClintHints)
            {
                responseHeaders.AppendList(
                                            HeaderNames.Vary,
                                            new[] {
                                                ClientHints.DPRHeader,
                                                ClientHints.WidthHeader,
                                                ClientHints.ViewportWidthHeader
                                            });
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

            //DPR
            if (cachedData.Metadata.DPR != null)
            {
                responseHeaders.Append("Content-DPR", cachedData.Metadata.DPR.Value.ToString(CultureInfo.InvariantCulture));
            }

            //is HEAD request?
            bool isHeadRequst = HttpMethods.IsHead(context.Request.Method);

            if (isHeadRequst == true)
            {
                return Results.Ok();
            }

            interceptors.Foreach(x => x.OnResponseSending(context.Response, cachedData));

            //send response stream
            Stream stream = await cachedData.OpenReadAsync();

            return Results.File(stream, cachedData.Metadata.MimeType, enableRangeProcessing: true);
        }
    }
}
