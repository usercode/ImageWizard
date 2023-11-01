// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Loaders;

/// <summary>
/// HttpLoaderSettings
/// </summary>
public class HttpLoaderOptions : LoaderOptions
{
    public HttpLoaderOptions()
    {
        RefreshMode = LoaderRefreshMode.None;
    }

    /// <summary>
    /// Custom http header entries.
    /// </summary>
    public IList<HttpHeaderItem> Headers { get; } = new List<HttpHeaderItem>();

    /// <summary>
    /// Sets default base url for relative urls.
    /// </summary>
    public string? DefaultBaseUrl { get; set; }

    /// <summary>
    /// Allows only absolute urls? (Relative urls use base url from request or DefaultBaseUrl from options.)
    /// </summary>
    public bool AllowAbsoluteUrls { get; set; } = false;

    /// <summary>
    /// Allows only specified hosts.
    /// </summary>
    public string[] AllowedHosts { get; set; } = Array.Empty<string>();

    public HttpLoaderOptions SetHeader(string name, string value)
    {
        Headers.Add(new HttpHeaderItem(name, value));

        return this;
    }
}
