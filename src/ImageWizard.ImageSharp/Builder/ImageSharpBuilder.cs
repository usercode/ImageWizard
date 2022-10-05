// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard;

class ImageSharpBuilder : PipelineBuilder, IImageSharpBuilder
{
    public ImageSharpBuilder(IServiceCollection services)
        : base(services)
    {
    }
}
