// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImageWizard.FFMpegCore.Builder;

class FFMpegBuilder : PipelineBuilder, IFFMpegBuilder
{
    public FFMpegBuilder(IServiceCollection services)
        : base(services)
    {
    }
}
