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
            wizardConfiguration.Services.AddHttpClient<GravatarLoader>();
            wizardConfiguration.ImageLoaderManager.Register<GravatarLoader>("gravatar");

            return wizardConfiguration;
        }
    }
}
