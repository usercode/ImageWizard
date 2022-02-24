// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Core;
using ImageWizard.FFMpegCore.Filters.Base;
using ImageWizard.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.FFMpegCore
{
    public class FFMpegPipeline : Pipeline<FFMpegFilter, FFMpegContext>
    {
        public FFMpegPipeline(
            IServiceProvider serviceProvider, 
            ILogger<FFMpegPipeline> logger, 
            IEnumerable<PipelineAction<FFMpegPipeline>> actions)
            : base(serviceProvider, logger)
        {
            actions.Foreach(x => x(this));
        }

        protected override Task<FFMpegContext> CreateFilterContext(PipelineContext context)
        {
            return Task.FromResult(new FFMpegContext(context));
        }
    }
}
