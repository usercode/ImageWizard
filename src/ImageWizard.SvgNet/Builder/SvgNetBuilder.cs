// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.SvgNet.Builder;

class SvgNetBuilder : PipelineBuilder, ISvgNetBuilder
{
    public SvgNetBuilder(IServiceCollection service)
        : base(service)
    {
    }      
}
