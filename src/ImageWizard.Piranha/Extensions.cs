using Microsoft.AspNetCore.Mvc;
using Piranha.Extend.Fields;
using System;
using Microsoft.AspNetCore.Html;
using ImageWizard.Client.Builder.Types;

namespace ImageWizard.Piranha
{
    public static class Extensions
    {
        public static IImageFilters Fetch(this IImageLoaderType imageBuilder, ImageField imageField)
        {
            imageBuilder.FetchLocalFile(imageBuilder.UrlHelper.Content(imageField.Media.PublicUrl));

            return (IImageFilters)imageBuilder;
        }
    }
}
