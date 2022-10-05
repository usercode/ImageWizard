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
        AllowAbsoluteUrls = false;
        AllowedHosts = Array.Empty<string>();
        RefreshMode = LoaderRefreshMode.None;
        Headers = new List<HttpHeaderItem>();
    }

    /// <summary>
    /// Custom http header entries.
    /// </summary>
    public IList<HttpHeaderItem> Headers { get; }

    /// <summary>
    /// Sets default base url for relative urls.
    /// </summary>
    public string? DefaultBaseUrl { get; set; }

    /// <summary>
    /// Allows only absolute urls? (Relative urls use base url from request or DefaultBaseUrl from options.)
    /// </summary>
    public bool AllowAbsoluteUrls { get; set; }

    /// <summary>
    /// Allows only specified hosts.
    /// </summary>
    public string[] AllowedHosts { get; set; }

    public HttpLoaderOptions SetHeader(string name, string value)
    {
        Headers.Add(new HttpHeaderItem(name, value));

        return this;
    }
}
