using ImageWizard.FFMpegCore.Filters;
using ImageWizard.FFMpegCore.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore.Builder
{
    class FFMpegBuilder : IImageSharpBuilder
    {
        public FFMpegBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            WithFilter<FrameFilter>();
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.LoaderManager => Builder.LoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public IImageSharpBuilder WithFilter<TFilter>() where TFilter : FFMpegFilter
        {
            Builder.Services.AddTransient<TFilter>();
            Builder.Services.AddSingleton(new PipelineAction<FFMpegPipeline>(x => x.AddFilter<TFilter>()));

            return this;
        }

        public IImageSharpBuilder WithOptions(Action<FFMpegOptions> action)
        {
            Builder.Services.Configure(action);

            return this;
        }
    }
}
