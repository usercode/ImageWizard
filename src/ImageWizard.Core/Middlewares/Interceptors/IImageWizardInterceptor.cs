using ImageWizard.Core.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core.Middlewares
{
    public interface IImageWizardInterceptor
    {
        void OnResponseSending(HttpResponse response, ICachedData cachedImage);

        void OnResponseCompleted(ICachedData cachedImage);

        void OnCachedImageCreated(ICachedData cachedImage);

        void OnFailedSignature();


    }
}
