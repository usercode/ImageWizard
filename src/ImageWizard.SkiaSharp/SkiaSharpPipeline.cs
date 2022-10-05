// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.SkiaSharp.Filters.Base;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace ImageWizard.SkiaSharp;

/// <summary>
/// SkiaPipeline
/// </summary>
public class SkiaSharpPipeline : Pipeline<SkiaSharpFilter, SkiaSharpFilterContext>
{
    public SkiaSharpPipeline(
        IServiceProvider serviceProvider, 
        ILogger<SkiaSharpPipeline> logger, 
        IEnumerable<PipelineAction<SkiaSharpPipeline>> actions)
        : base(serviceProvider, logger)
    {
        actions.Foreach(x => x(this));
    }

    protected override async Task<SkiaSharpFilterContext> CreateFilterContext(PipelineContext context)
    {
        IImageFormat? targetFormat = null;

        if (context.ImageWizardOptions.UseAcceptHeader)
        {
            //try to use mime type from accept header
            targetFormat = ImageFormatHelper.FirstOrDefault(context.AcceptMimeTypes);
        }

        if (targetFormat == null)
        {
            //use mime type of the original image
            targetFormat = ImageFormatHelper.FirstOrDefault(context.Result.MimeType);
        }

        SKBitmap bitmap = SKBitmap.Decode(context.Result.Data);

        if (bitmap == null)
        {
            throw new Exception("SkiaSharp could not load the image.");
        }

        return new SkiaSharpFilterContext(context, bitmap, targetFormat);
    }
}
