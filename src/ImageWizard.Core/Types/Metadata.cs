// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// Metadata
/// </summary>
public class Metadata : IMetadata
{
    public Metadata()
    {
        Key = string.Empty;
        Hash = string.Empty;        
        Filters = Array.Empty<string>();
        LoaderType = string.Empty;
        LoaderSource = string.Empty;
        MimeType = MimeTypes.Object;

        Cache = new CacheSettings();
    }

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
    public string Key { get; set; }

    /// <summary>
    /// Hash
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// MimeType
    /// </summary>
    public string MimeType { get; set; }

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
    public CacheSettings Cache { get; set; }

    /// <summary>
    /// Filters
    /// </summary>
    public IEnumerable<string> Filters { get; set; }

    /// <summary>
    /// LoaderType
    /// </summary>
    public string LoaderType { get; set; }

    /// <summary>
    /// LoaderSource
    /// </summary>
    public string LoaderSource { get; set; }
}
