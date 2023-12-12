// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.OpenCvSharp.Filters;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;

namespace ImageWizard.ImageSharp.Filters;

/// <summary>
/// ImageSharpPipeline
/// </summary>
public class OpenCvSharpPipeline : Pipeline<OpenCvSharpFilter, OpenCvSharpFilterContext>
{
    public OpenCvSharpPipeline(
        IServiceProvider service, 
        ILogger<OpenCvSharpPipeline> logger, 
        IEnumerable<PipelineAction<OpenCvSharpPipeline>> actions)
        : base(service, logger)
    {
        actions.Foreach(x => x(this));
    }

    protected override Task<OpenCvSharpFilterContext> CreateFilterContext(PipelineContext context)
    {            
        return Task.FromResult(new OpenCvSharpFilterContext(context));
    }
}
