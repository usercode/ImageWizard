using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public interface IImageWizardInterceptor
    {
        void OnResponseSending(HttpResponse response, ICachedData cachedImage);

        void OnCachedImageCreated(ICachedData cachedImage);

        void OnFailedSignature();

    }
}
