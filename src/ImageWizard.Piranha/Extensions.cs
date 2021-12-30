using Microsoft.AspNetCore.Mvc;
using Piranha.Extend.Fields;
using System;
using Microsoft.AspNetCore.Html;
using ImageWizard.Client.Builder.Types;
using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.Piranha
{
    public static class Extensions
    {
        public static IFilter Fetch(this ILoader imageLoader, ImageField imageField)
        {
            //UrlDecode: fix whitespace handling for correct signature check
            IUrlHelper urlHelper = imageLoader.ServiceProvider.GetRequiredService<IUrlHelper>();

            imageLoader.FetchLocalFile(urlHelper.Content(HttpUtility.UrlDecode(imageField.Media.PublicUrl)));

            return (IFilter)imageLoader;
        }
    }
}
