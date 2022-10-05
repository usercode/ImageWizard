// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// ICachedData
/// </summary>
public interface ICachedData
{
    /// <summary>
    /// Metadata
    /// </summary>
    /// <returns></returns>
    IMetadata Metadata { get; }

    /// <summary>
    /// OpenReadAsync
    /// </summary>
    /// <returns></returns>
    Task<Stream> OpenReadAsync();
}
