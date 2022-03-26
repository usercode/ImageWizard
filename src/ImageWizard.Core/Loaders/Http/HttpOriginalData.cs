// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Loaders;

/// <summary>
/// HttpOriginalData
/// </summary>
public class HttpOriginalData : OriginalData
{
    public HttpOriginalData(HttpResponseMessage response, string mimeType, Stream data, CacheSettings cacheSettings)
        : base(mimeType, data, cacheSettings)
    {
        Response = response;
    }

    /// <summary>
    /// HttpResponseMessage
    /// </summary>
    private HttpResponseMessage Response { get; }

    private bool _disposed;

    public override void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        base.Dispose();

        Response.Dispose();

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
