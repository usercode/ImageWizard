﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;
using System.Text.RegularExpressions;

namespace ImageWizard;

/// <summary>
/// ImageWizardUrl
/// </summary>
public readonly partial struct ImageWizardUrl
{
	[GeneratedRegex("^(?<path>(?<filter>[a-z]+\\([^)]*\\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.IgnoreCase)]
	private static partial Regex MyRegex();

    public ImageWizardUrl(string loaderType, string loaderSource, FilterSegment[] filters)
    {
        LoaderType = loaderType;
        LoaderSource = loaderSource.TrimStart('/');
        Filters = filters;

        if (Filters.Length > 0)
        {
            Path = $"{string.Join('/', Filters)}/{LoaderType}/{LoaderSource}";
        }
        else
        {
            Path = $"{LoaderType}/{LoaderSource}";
        }
    }

    private ImageWizardUrl(string path, string loaderType, string loaderSource, FilterSegment[] filters)
    {
        LoaderType = loaderType;
        LoaderSource = loaderSource;
        Filters = filters;
        Path = path;
    }

    public static bool TryParse(string path, out ImageWizardUrl url)
    {
        Match match = MyRegex().Match(path);

        if (match.Success == false)
        {
            url = new ImageWizardUrl();

            return false;
        }

        string url_path = match.Groups["path"].Value;
        string url_loaderSource = match.Groups["loaderSource"].Value;
        string url_loaderType = match.Groups["loaderType"].Value;
        FilterSegment[] url_filters = match.Groups["filter"].Captures
                                                            .Select(x => x.ValueSpan[0..^1].ToString()) //remove "/"
                                                            .Select(x => FilterSegment.FromFilter(x))
                                                            .ToArray();

        url = new ImageWizardUrl(url_path, url_loaderType, url_loaderSource, url_filters);

        return true;
    }

    /// <summary>
    /// Path
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// LoaderType
    /// </summary>
    public string LoaderType { get; }

    /// <summary>
    /// LoaderSource
    /// </summary>
    public string LoaderSource { get; }

    /// <summary>
    /// Filters
    /// </summary>
    public FilterSegment[] Filters { get; }

    public override string ToString()
    {
        return Path;
    }
}
