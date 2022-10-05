// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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

    public void OnCachedDataCreated(ICachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataCreated(cachedData));
    }

    public void OnCachedDataDeleted(ICachedData cachedData)
    {
        Items.Foreach(x => x.OnCachedDataDeleted(cachedData));
    }

    public void OnCachedDataSent(ICachedData cachedData, bool notModified)
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
