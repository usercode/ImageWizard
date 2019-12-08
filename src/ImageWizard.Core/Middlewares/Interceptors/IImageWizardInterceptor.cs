using ImageWizard.Core.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Middlewares
{
    public interface IImageWizardInterceptor
    {
        void OnResponseSending(HttpResponse response, ICachedImage cachedImage);

        void OnResponseCompleted(ICachedImage cachedImage);

        void OnCachedImageCreated(ICachedImage cachedImage);

        void OnFailedSignature();


    }
}
