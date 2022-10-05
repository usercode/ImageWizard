// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageWizard.DocNET;

/// <summary>
/// DocNETPipeline
/// </summary>
public class DocNETPipeline : Pipeline<DocNETFilter, DocNETFilterContext>
{
    public DocNETPipeline(
        IServiceProvider service, 
        ILogger<DocNETPipeline> logger, 
        IEnumerable<PipelineAction<DocNETPipeline>> actions)
        : base(service, logger)
    {
        actions.Foreach(x => x(this));
    }

    protected override Task<DocNETFilterContext> CreateFilterContext(PipelineContext context)
    {
        return Task.FromResult(new DocNETFilterContext(context, context.Result.Data));
    }
}
