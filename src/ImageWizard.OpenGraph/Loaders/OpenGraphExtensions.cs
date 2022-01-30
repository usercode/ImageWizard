using ImageWizard.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class OpenGraphExtensions
    {
        public static IImageWizardBuilder AddOpenGraphLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddOpenGraphLoader(wizardConfiguration, x => { });
        }

        public static IImageWizardBuilder AddOpenGraphLoader(this IImageWizardBuilder wizardConfiguration, Action<OpenGraphOptions> options)
        {
            wizardConfiguration.Services.Configure(options);
            wizardConfiguration.Services.AddHttpClient<OpenGraphLoader>();
            wizardConfiguration.LoaderManager.Register<OpenGraphLoader>("opengraph");

            return wizardConfiguration;
        }
    }
}
