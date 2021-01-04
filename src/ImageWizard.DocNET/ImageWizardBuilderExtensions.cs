using ImageWizard.Core.ImageFilters.Base;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Core.Types;
using ImageWizard.DocNET.Builder;
using ImageWizard.Filters;
using ImageWizard.Filters.ImageFormats;
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
