// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Processing.Results;

/// <summary>
/// DataResult
/// </summary>
public class DataResult : IDisposable
{
    public DataResult(Stream data, string mimeType)
    {
        MimeType = mimeType;
        Data = data;
    }

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType { get; }

    /// <summary>
    /// Data
    /// </summary>
    public Stream Data { get; }

    private bool _disposed;

    public virtual void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Data.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
