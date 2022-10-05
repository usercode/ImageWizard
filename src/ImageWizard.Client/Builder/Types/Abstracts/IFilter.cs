// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// IFilter
/// </summary>
public interface IFilter : IBuildUrl, IUrlBuilder
{
    /// <summary>
    /// Filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IFilter Filter(string filter);
}
