// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core.Processing.Builder;
using ImageWizard.FFMpegCore.Filters;
using ImageWizard.FFMpegCore.Filters.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.FFMpegCore.Builder;

class FFMpegBuilder : PipelineBuilder, IFFMpegBuilder
{
    public FFMpegBuilder(IServiceCollection services)
        : base(services)
    {
    }
}
