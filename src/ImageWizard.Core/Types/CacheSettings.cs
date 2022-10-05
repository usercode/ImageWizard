// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

/// <summary>
/// CacheSettings
/// </summary>
public class CacheSettings
{
    public CacheSettings()
    {

    }

    public bool NoCache { get; set; }
    public bool NoStore { get; set; }
    public string? ETag { get; set; }
    public DateTime? Expires { get; set; }

}
