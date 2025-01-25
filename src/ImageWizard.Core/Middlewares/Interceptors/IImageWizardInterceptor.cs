// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard;

public interface IImageWizardInterceptor
{
    void OnCachedDataSent(CachedData cachedData, bool notModified);

    void OnCachedDataCreated(CachedData cachedData);

    void OnCachedDataDeleted(CachedData cachedData);

    void OnValidSignature(ImageWizardUrl url);

    void OnInvalidSignature(ImageWizardUrl url);

    void OnInvalidUrl(string path);
}
