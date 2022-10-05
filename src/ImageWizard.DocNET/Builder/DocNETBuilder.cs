// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.DocNET.Builder;

class DocNETBuilder : PipelineBuilder, IDocNETBuilder
{
    public DocNETBuilder(IServiceCollection services)
        : base(services)
    {
    }
}
