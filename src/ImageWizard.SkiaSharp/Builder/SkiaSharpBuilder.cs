// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.SkiaSharp.Builder;

class SkiaSharpBuilder : PipelineBuilder, ISkiaSharpBuilder
{
    public SkiaSharpBuilder(IServiceCollection services)
        :base(services)
    {
    }
}
