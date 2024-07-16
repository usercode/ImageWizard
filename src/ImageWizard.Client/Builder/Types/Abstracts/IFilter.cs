// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Utils;

namespace ImageWizard.Client;

/// <summary>
/// IFilter
/// </summary>
public interface IFilter : IBuildUrl, IUrlBuilder
{
    /// <summary>
    /// Filter
    /// </summary>
    IFilter Filter(FilterSegment filter);
}
