// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageWizard.ImageSharp.Filters;

/// <summary>
/// ImageSharpPipeline
/// </summary>
public class ImageSharpPipeline : Pipeline<ImageSharpFilter, ImageSharpFilterContext>
{
    public ImageSharpPipeline(
        IServiceProvider service, 
        IOptions<ImageSharpOptions> options,
        ILogger<ImageSharpPipeline> logger, 
        IEnumerable<PipelineAction<ImageSharpPipeline>> actions)
        : base(service, logger)
    {
        Options = options;

        actions.Foreach(x => x(this));
    }

    /// <summary>
    /// Options
    /// </summary>
    private IOptions<ImageSharpOptions> Options { get; }

    protected override async Task<ImageSharpFilterContext> CreateFilterContext(PipelineContext context)
    {
        Image image = await Image.LoadAsync(context.Result.Data);

        IImageFormat? targetFormat = null;

        if (context.ImageWizardOptions.UseAcceptHeader)
        {
            targetFormat = ImageFormatHelper.FirstOrDefault(context.AcceptMimeTypes);
        }

        if (targetFormat == null)
        {
            targetFormat = ImageFormatHelper.FirstOrDefault(context.Result.MimeType);
        }

        if (targetFormat == null)
        {
            throw new Exception("No image format is selected.");
        }

        ImageSharpFilterContext imageSharpContext = new ImageSharpFilterContext(context, image, targetFormat, Options);

        return imageSharpContext;
    }
}
