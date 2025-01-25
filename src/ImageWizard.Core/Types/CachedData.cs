// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public delegate Task<Stream> OpenStreamHandler();

/// <summary>
/// CachedData
/// </summary>
public class CachedData
{
    public CachedData(Metadata metadata, OpenStreamHandler streamTask)
    {
        Metadata = metadata;
        StreamTask = streamTask;
    }

    /// <summary>
    /// Metadata
    /// </summary>
    public Metadata Metadata { get; }

    /// <summary>
    /// Buffer
    /// </summary>
    private OpenStreamHandler StreamTask { get; }

    /// <summary>
    /// Opens readonly stream
    /// </summary>
    /// <returns></returns>
    public Task<Stream> OpenReadAsync()
    {
        return StreamTask();
    }
}
