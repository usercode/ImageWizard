using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public interface IImageUrlBuilder
    {
        IWebHostEnvironment HostingEnvironment { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
        IFileVersionProvider FileVersionProvider { get; }
        IUrlHelper UrlHelper { get; }
    }
}
