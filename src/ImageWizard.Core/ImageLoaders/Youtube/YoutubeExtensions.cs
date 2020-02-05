using ImageWizard.Core.ImageLoaders.Youtube;
using ImageWizard.Core.Middlewares;
using ImageWizard.ImageLoaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    public static class YouTubeExtensions
    {
        public static IImageWizardBuilder AddYoutubeLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddYoutubeLoader(wizardConfiguration, x => { });
        }

        public static IImageWizardBuilder AddYoutubeLoader(this IImageWizardBuilder wizardConfiguration, Action<YouTubeOptions> options)
        {
            wizardConfiguration.Services.Configure(options);
            wizardConfiguration.Services.AddHttpClient2<YouTubeLoader>();
            wizardConfiguration.ImageLoaderManager.Register<YouTubeLoader>("youtube");

            return wizardConfiguration;
        }
    }
}
