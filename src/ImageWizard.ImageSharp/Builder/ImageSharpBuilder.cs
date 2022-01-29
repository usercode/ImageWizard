using ImageWizard.ImageSharp.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    class ImageSharpBuilder : IImageSharpBuilder
    {
        public ImageSharpBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            this.WithFilter<ResizeFilter>();
            this.WithFilter<BackgroundColorFilter>();
            this.WithFilter<CropFilter>();
            this.WithFilter<GrayscaleFilter>();
            this.WithFilter<BlackWhiteFilter>();
            this.WithFilter<TrimFilter>();
            this.WithFilter<FlipFilter>();
            this.WithFilter<RotateFilter>();
            this.WithFilter<BlurFilter>();
            this.WithFilter<InvertFilter>();
            this.WithFilter<BrightnessFilter>();
            this.WithFilter<ContrastFilter>();
            this.WithFilter<DPRFilter>();
            this.WithFilter<AutoOrientFilter>();
            this.WithFilter<MetadataFilter>();
            this.WithFilter<ImageFormatFilter>();
            this.WithFilter<TextFilter>();
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.LoaderManager => Builder.LoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }
    }
}
