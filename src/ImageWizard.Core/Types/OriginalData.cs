// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard;

/// <summary>
/// OriginalData
/// </summary>
public class OriginalData : IDisposable
{
    public OriginalData(string mimeType, Stream data)
        : this(mimeType, data, new CacheSettings())
    {
    }

    public OriginalData(string mimeType, Stream data, CacheSettings cacheSettings)
    {
        MimeType = mimeType;
        Data = data;

        Cache = cacheSettings ?? throw new ArgumentNullException(nameof(cacheSettings));
    }

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType { get; }

    /// <summary>
    /// Data
    /// </summary>
    public Stream Data { get; }

    /// <summary>
    /// Cache
    /// </summary>
    public CacheSettings Cache { get; }

    private bool disposed;

    public virtual void Dispose()
    {
        if (disposed)
        {
            return;
        }

        Data.Dispose();
        disposed = true;

        GC.SuppressFinalize(this);
    }
}
