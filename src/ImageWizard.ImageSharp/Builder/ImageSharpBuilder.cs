using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.ImageSharp.Builder
{
    class ImageSharpBuilder : IImageSharpBuilder
    {
        public ImageSharpBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            WithFilter<ResizeFilter>();
            WithFilter<BackgroundColorFilter>();
            WithFilter<CropFilter>();
            WithFilter<GrayscaleFilter>();
            WithFilter<BlackWhiteFilter>();
            WithFilter<TrimFilter>();
            WithFilter<FlipFilter>();
            WithFilter<RotateFilter>();
            WithFilter<BlurFilter>();
            WithFilter<InvertFilter>();
            WithFilter<BrightnessFilter>();
            WithFilter<ContrastFilter>();
            WithFilter<DPRFilter>();
            WithFilter<AutoOrientFilter>();
            WithFilter<MetadataFilter>();
            WithFilter<ImageFormatFilter>();
            WithFilter<TextFilter>();
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.LoaderManager => Builder.LoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public IImageSharpBuilder WithFilter<TFilter>() where TFilter : ImageSharpFilter
        {
            Builder.Services.AddTransient<TFilter>();
            Builder.Services.AddSingleton(new PipelineAction<ImageSharpPipeline>(x => x.AddFilter<TFilter>()));

            return this;
        }

        public IImageSharpBuilder WithOptions(Action<ImageSharpOptions> action)
        {
            Builder.Services.Configure(action);

            return this;
        }
    }
}
