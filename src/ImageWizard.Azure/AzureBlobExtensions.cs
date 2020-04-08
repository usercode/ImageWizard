using ImageWizard.Core.Middlewares;
using ImageWizard.ImageLoaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Azure
{
    public static class AzureBlobExtensions
    {
        public static IImageWizardBuilder AddAzureBlob(this IImageWizardBuilder wizardConfiguration)
        {
            return AddAzureBlob(wizardConfiguration, x => { });
        }
        
        public static IImageWizardBuilder AddAzureBlob(this IImageWizardBuilder wizardConfiguration, Action<AzureBlobOptions> setup)
        {
            wizardConfiguration.Services.Configure(setup);

            wizardConfiguration.Services.AddSingleton<AzureBlobLoader>();
            wizardConfiguration.ImageLoaderManager.Register<AzureBlobLoader>("azure");

            return wizardConfiguration;
        }
    }
}
