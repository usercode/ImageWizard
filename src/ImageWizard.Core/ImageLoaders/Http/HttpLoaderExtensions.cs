using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageLoaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageWizard
{
    public static class HttpLoaderExtensions
    {
        public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddHttpLoader(wizardConfiguration, options => { });
        }

        public static IImageWizardBuilder AddHttpLoader(this IImageWizardBuilder wizardConfiguration, Action<HttpLoaderSettings> setup)
        {
            wizardConfiguration.Services.AddHttpContextAccessor();

            wizardConfiguration.Services.Configure(setup);
            wizardConfiguration.Services.AddHttpClient<HttpLoader>();
            wizardConfiguration.ImageLoaderManager.Register<HttpLoader>("fetch");

            return wizardConfiguration;
        }
    }
}
