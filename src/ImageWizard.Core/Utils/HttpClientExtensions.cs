// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Net.Http.Headers;

namespace ImageWizard;

public static class HttpClientExtensions
{
    public static void SetUserAgentHeader(this HttpRequestMessage request)
    {
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("ImageWizard", "3.0"));
    }

    public static void SetIfNoneMatch(this HttpRequestMessage request, CachedData? cachedData)
    {
        if (cachedData != null)
        {
            if (string.IsNullOrEmpty(cachedData.Metadata.Cache.ETag) == false)
            {
                request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue($"\"{cachedData.Metadata.Cache.ETag}\""));
            }
        }
    }
}
