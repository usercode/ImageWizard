using Microsoft.Extensions.DependencyInjection;
using System;

namespace ImageWizard.AWS
{
    public static class AwsExtensions
    {
        public static IImageWizardBuilder AddAWS(this IImageWizardBuilder wizardConfiguration)
        {
            return AddAWS(wizardConfiguration, x => { });
        }

        public static IImageWizardBuilder AddAWS(this IImageWizardBuilder wizardConfiguration, Action<AwsOptions> setup)
        {
            wizardConfiguration.Services.Configure(setup);

            wizardConfiguration.Services.AddSingleton<AwsLoader>();
            wizardConfiguration.LoaderManager.Register<AwsLoader>("aws");

            return wizardConfiguration;
        }
    }
}
