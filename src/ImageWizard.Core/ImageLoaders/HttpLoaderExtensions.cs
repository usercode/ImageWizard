using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageLoaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class HttpLoaderExtensions
    {
        public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddHttpLoader(wizardConfiguration, new HttpLoaderSettings());
        }

        public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration, HttpLoaderSettings settings)
        {
            wizardConfiguration.Services.AddSingleton(settings);
            wizardConfiguration.Services.AddHttpClient<IImageLoader, HttpLoader>();

            return wizardConfiguration;
        }
    }
}
