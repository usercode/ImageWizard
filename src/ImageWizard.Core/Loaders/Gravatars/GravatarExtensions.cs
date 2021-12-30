using ImageWizard.Core.ImageLoaders.Gravatars;
using ImageWizard.Core.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class GravatarExtensions
    {
        public static IImageWizardBuilder AddGravatarLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddGravatarLoader(wizardConfiguration, x => { });
        }

        public static IImageWizardBuilder AddGravatarLoader(this IImageWizardBuilder wizardConfiguration, Action<GravatarOptions> options)
        {
            wizardConfiguration.Services.Configure(options);
            wizardConfiguration.Services.AddHttpClient<GravatarLoader>();
            wizardConfiguration.ImageLoaderManager.Register<GravatarLoader>("gravatar");

            return wizardConfiguration;
        }
    }
}
