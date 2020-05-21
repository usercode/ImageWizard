using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
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
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.ImageLoaderManager => Builder.ImageLoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public ISkiaSharpBuilder WithFilter<TFilter>() where TFilter : SkiaSharpFilter, new()
        {
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
