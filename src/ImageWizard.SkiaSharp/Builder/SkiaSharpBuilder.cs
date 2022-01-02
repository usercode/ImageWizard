using ImageWizard.SkiaSharp.Filters;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.SkiaSharp.Builder
{
    class SkiaSharpBuilder : ISkiaSharpBuilder
    {
        public SkiaSharpBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            WithFilter<ResizeFilter>();
            WithFilter<RotateFilter>();
            WithFilter<CropFilter>();
            WithFilter<GrayscaleFilter>();
            WithFilter<BlurFilter>();
            WithFilter<FlipFilter>();
            WithFilter<DPRFilter>();
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

        public ISkiaSharpBuilder WithFilter<TFilter>() where TFilter : SkiaSharpFilter, new()
        {
            Builder.Services.AddTransient<TFilter>();
            Builder.Services.AddSingleton(new PipelineAction<SkiaSharpPipeline>(x => x.AddFilter<TFilter>()));

            return this;
        }

        public ISkiaSharpBuilder WithOptions(Action<SkiaSharpOptions> action)
        {
            Builder.Services.Configure(action);

            return this;
        }
    }
}
