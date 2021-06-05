using Microsoft.AspNetCore.Mvc;
using Piranha.Extend.Fields;
using System;
using Microsoft.AspNetCore.Html;
using ImageWizard.Client.Builder.Types;
using System.Web;

namespace ImageWizard.Piranha
{
    public static class Extensions
    {
        public static IImageFilters Fetch(this IImageLoaderType imageBuilder, ImageField imageField)
        {
            //UrlDecode: fix whitespace handling for correct signature check
            imageBuilder.FetchLocalFile(imageBuilder.UrlHelper.Content(HttpUtility.UrlDecode(imageField.Media.PublicUrl)));

            return (IImageFilters)imageBuilder;
        }
    }
}
