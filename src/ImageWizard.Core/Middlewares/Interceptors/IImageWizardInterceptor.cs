// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public interface IImageWizardInterceptor
{
    void OnCachedDataSent(ICachedData cachedData, bool notModified);

    void OnCachedDataCreated(ICachedData cachedData);

    void OnCachedDataDeleted(ICachedData cachedData);

    void OnUnsafeSignature();

    void OnValidSignature();

    void OnInvalidSignature();

}
