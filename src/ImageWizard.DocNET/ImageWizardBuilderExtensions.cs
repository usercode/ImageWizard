using ImageWizard.Core.Middlewares;
using ImageWizard.DocNET.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.DocNET
{
    /// <summary>
    /// FilterExtensions
    /// </summary>
    public static class ImageWizardBuilderExtensions
    {
        public static IImageWizardBuilder AddDocNET(this IImageWizardBuilder builder)
        {
            return AddDocNET(builder, MimeTypes.Pdf);
        }

        public static IDocNETBuilder AddDocNET(this IImageWizardBuilder builder, params string[] mimeTypes)
        {
            builder.AddPipeline<DocNETPipeline>(mimeTypes);

            return new DocNETBuilder(builder);
        }
    }
}
