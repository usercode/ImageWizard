// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// Metadata
/// </summary>
public class Metadata : IMetadata
{
    /// <summary>
    /// CreatedAt
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// LastAccess
    /// </summary>
    public DateTime LastAccess { get; set; }

    /// <summary>
    /// Signature
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Hash
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType { get; set; } = MimeTypes.Object;

    /// <summary>
    /// Width
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Height
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// FileLength
    /// </summary>
    public long FileLength { get; set; }

    /// <summary>
    /// CacheSettings
    /// </summary>
    public CacheSettings Cache { get; set; } = new CacheSettings();

    /// <summary>
    /// Filters
    /// </summary>
    public IEnumerable<string> Filters { get; set; } = Array.Empty<string>();

    /// <summary>
    /// LoaderType
    /// </summary>
    public string LoaderType { get; set; } = string.Empty;

    /// <summary>
    /// LoaderSource
    /// </summary>
    public string LoaderSource { get; set; } = string.Empty;
}
