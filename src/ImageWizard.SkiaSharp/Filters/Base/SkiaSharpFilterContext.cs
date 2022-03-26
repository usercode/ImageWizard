// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.SkiaSharp.Filters.Base;

/// <summary>
/// SkiaSharpFilterContext
/// </summary>
public class SkiaSharpFilterContext : FilterContext
{
    public SkiaSharpFilterContext(PipelineContext processingContext, SKBitmap image, IImageFormat imageFormat)
        : base(processingContext)
    {
        Image = image;
        ImageFormat = imageFormat;
    }

    private SKBitmap _image;

    /// <summary>
    /// Image
    /// </summary>
    public SKBitmap Image 
    {
        get => _image;
        set
        {
            _image?.Dispose();

            _image = value;
        }
    }

    /// <summary>
    /// ImageFormat
    /// </summary>
    public IImageFormat ImageFormat { get; set; }

    private bool _disposed;

    public override void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Image.Dispose();

        _disposed = true;

        GC.SuppressFinalize(this);
    }

    public override async Task<DataResult> BuildResultAsync()
    {
        Stream mem = ProcessingContext.StreamPool.GetStream();

        ImageFormat.SaveImage(Image, mem);

        mem.Seek(0, SeekOrigin.Begin);

        return new ImageResult(
                                mem,
                                ImageFormat.MimeType,
                                Image.Width,
                                Image.Height);
    }
}
