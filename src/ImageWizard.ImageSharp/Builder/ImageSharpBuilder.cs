using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
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
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.ImageLoaderManager => Builder.ImageLoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public IImageSharpBuilder WithFilter<TFilter>() where TFilter : ImageSharpFilter, new()
        {
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
