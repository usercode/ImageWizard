using ImageWizard.ImageSharp.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// FilterExtensions
    /// </summary>
    public static class ImageWizardBuilderExtensions
    {
        public static IImageSharpBuilder AddImageSharp(this IImageWizardBuilder builder)
        {
            return AddImageSharp(builder, MimeTypes.Jpeg, MimeTypes.Png, MimeTypes.Gif, MimeTypes.Bmp);
        }

        public static IImageSharpBuilder AddImageSharp(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<ImageSharpPipeline>(mimeTypes);

            return new ImageSharpBuilder(builder);
        }
    }
}
