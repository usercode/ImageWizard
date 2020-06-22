using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.Filters;
using ImageWizard.SvgNet.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SvgNet.Builder
{
    class SvgNetBuilder : ISvgNetBuilder
    {
        public SvgNetBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            WithFilter<RemoveSizeFilter>();
            WithFilter<RotateFilter>();
            WithFilter<BlurFilter>();
            WithFilter<GrayscaleFilter>();
            WithFilter<InvertFilter>();
            WithFilter<SaturateFilter>();
            WithFilter<ImageFormatFilter>();
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.ImageLoaderManager => Builder.ImageLoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public ISvgNetBuilder WithFilter<TFilter>() where TFilter : SvgFilter, new()
        {
            Builder.Services.AddTransient<TFilter>();
            Builder.Services.AddSingleton(new PipelineAction<SvgPipeline>(x => x.AddFilter<TFilter>()));

            return this;
        }

        public ISvgNetBuilder WithOptions(Action<SvgOptions> action)
        {
            Builder.Services.Configure(action);

            return this;
        }
    }
}
