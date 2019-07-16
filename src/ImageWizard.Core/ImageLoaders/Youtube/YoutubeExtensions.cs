using ImageWizard.Core.ImageLoaders.Youtube;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageLoaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class YoutubeExtensions
    {
        public static IImageWizardBuilder AddYoutubeLoader(this IImageWizardBuilder wizardConfiguration)
        {
            wizardConfiguration.Services.AddHttpClient<IImageLoader, YoutubeLoader>();

            return wizardConfiguration;
        }
    }
}
