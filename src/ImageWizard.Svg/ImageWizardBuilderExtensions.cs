using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Types;
using ImageWizard.SvgNet.Filters;
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
        public static IImageWizardBuilder AddSvgNet(this IImageWizardBuilder builder)
        {
            builder.AddPipeline<SvgPipeline>(new[] { MimeTypes.Svg });

            return builder;
        }
    }
}
