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
    public static IImageWizardBuilder AddAzureLoader(this IImageWizardBuilder wizardConfiguration)
    {
        return AddAzureLoader(wizardConfiguration, x => { });
    }
    
    public static IImageWizardBuilder AddAzureLoader(this IImageWizardBuilder wizardConfiguration, Action<AzureBlobOptions> setup)
    {
        wizardConfiguration.Services.Configure(setup);

        wizardConfiguration.Services.AddSingleton<AzureBlobLoader>();
        wizardConfiguration.LoaderManager.Register<AzureBlobLoader>("azure");

        return wizardConfiguration;
    }
}
