// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Helpers;

namespace ImageWizard;

/// <summary>
/// InterceptorInvoker
/// </summary>
public class InterceptorInvoker : IImageWizardInterceptor
{
    public InterceptorInvoker(IEnumerable<IImageWizardInterceptor> items)
    {
        Items = items;
    }

    /// <summary>
    /// Items
    /// </summary>
    private IEnumerable<IImageWizardInterceptor> Items { get; }

    public void OnCachedDataCreated(CachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataCreated(cachedData));
    }

    public void OnCachedDataDeleted(CachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataDeleted(cachedData));
    }

    public void OnCachedDataSent(CachedData cachedData, bool notModified)
    {
        Items.Foreach(x => x.OnCachedDataSent(cachedData, notModified));
    }

    public void OnInvalidSignature(ImageWizardUrl url)
    {
        Items.Foreach(x => x.OnInvalidSignature(url));
    }

    public void OnValidSignature(ImageWizardUrl url)
    {
        Items.Foreach(x => x.OnValidSignature(url));
    }

    public void OnInvalidUrl(string path)
    {
        Items.Foreach(x => x.OnInvalidUrl(path));
    }
}
