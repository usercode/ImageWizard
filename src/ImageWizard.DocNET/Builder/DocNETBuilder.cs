using ImageWizard.Core.ImageLoaders;
using ImageWizard.Core.Middlewares;
using ImageWizard.Core.Settings;
using ImageWizard.DocNET.Filters;
using ImageWizard.DocNET.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Builder
{
    class DocNETBuilder : IDocNETBuilder
    {
        public DocNETBuilder(IImageWizardBuilder builder)
        {
            Builder = builder;

            WithFilter<PageToImageFilter>();
            WithFilter<SubPagesFilter>();
        }

        private IImageWizardBuilder Builder { get; }

        IServiceCollection IImageWizardBuilder.Services => Builder.Services;

        TypeManager IImageWizardBuilder.ImageLoaderManager => Builder.ImageLoaderManager;

        TypeManager IImageWizardBuilder.PipelineManager => Builder.PipelineManager;

        void IImageWizardBuilder.AddPipeline<T>(string[] mimeTypes)
        {
            Builder.AddPipeline<T>(mimeTypes);
        }

        public IDocNETBuilder WithFilter<TFilter>() where TFilter : DocNETFilter
        {
            Builder.Services.AddTransient<TFilter>();
            Builder.Services.AddSingleton(new PipelineAction<DocNETPipeline>(x => x.AddFilter<TFilter>()));

            return this;
        }

        //public IDocNETBuilder WithOptions(Action<DocNETOptions> action)
        //{
        //    Builder.Services.Configure(action);

        //    return this;
        //}
    }
}
