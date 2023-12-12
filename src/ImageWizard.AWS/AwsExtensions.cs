// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.AWS;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

public static class AwsExtensions
{
    public static IImageWizardBuilder AddAWSLoader(this IImageWizardBuilder wizardConfiguration, Action<AwsOptions>? options = null)
    {
        if (options != null)
        {
            wizardConfiguration.Services.Configure(options);
        }

        wizardConfiguration.Services.AddSingleton<AwsLoader>();
        wizardConfiguration.LoaderManager.Register<AwsLoader>("aws");

        return wizardConfiguration;
    }
}
