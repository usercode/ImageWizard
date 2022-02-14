using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public interface IImageWizardInterceptor
    {
        void OnCachedDataSending(HttpResponse response, ICachedData cachedData, bool notModified);

        void OnCachedDataCreated(ICachedData cachedData);

        void OnUnsafeSignature();

        void OnValidSignature();

        void OnInvalidSignature();

    }
}
