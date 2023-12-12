// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Filters;
using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

/// <summary>
/// ImageSharpPipeline
/// </summary>
public class SvgPipeline : Pipeline<SvgFilter, SvgFilterContext>
{
    public SvgPipeline(
        IServiceProvider serviceProvider,
        ILogger<SvgPipeline> logger, 
        IEnumerable<PipelineAction<SvgPipeline>> actions)
        : base(serviceProvider, logger)
    {
        actions.Foreach(x => x(this));
    }

    protected override async Task<SvgFilterContext> CreateFilterContext(PipelineContext context)
    {
        //load image
        XDocument svg = XDocument.Load(context.Result.Data);

        return new SvgFilterContext(context, svg);
    }
}
