// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Azure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard;

public static class AzureBlobExtensions
{    
    public static IImageWizardBuilder AddAzureLoader(this IImageWizardBuilder wizardConfiguration, Action<AzureBlobOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddSingleton<AzureBlobLoader>();
        wizardConfiguration.LoaderManager.Register<AzureBlobLoader>("azure");

        return wizardConfiguration;
    }
}
