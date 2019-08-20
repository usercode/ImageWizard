using ImageWizard.Core.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class ImageWizardInterceptosExtensions
    {
        public static IImageWizardBuilder AddInterceptor<TInterceptor>(this IImageWizardBuilder builder)
          where TInterceptor : class, IImageWizardInterceptor
        {
            builder.Services.AddTransient<IImageWizardInterceptor, TInterceptor>();

            return builder;
        }
    }
}
