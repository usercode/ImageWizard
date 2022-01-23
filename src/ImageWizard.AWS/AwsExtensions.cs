﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard.AWS
{
    public static class AwsExtensions
    {
        public static IImageWizardBuilder AddAWSLoader(this IImageWizardBuilder wizardConfiguration)
        {
            return AddAWSLoader(wizardConfiguration, x => { });
        }

        public static IImageWizardBuilder AddAWSLoader(this IImageWizardBuilder wizardConfiguration, Action<AwsOptions> setup)
        {
            wizardConfiguration.Services.Configure(setup);

            wizardConfiguration.Services.AddSingleton<AwsLoader>();
            wizardConfiguration.LoaderManager.Register<AwsLoader>("aws");

            return wizardConfiguration;
        }
    }
}
