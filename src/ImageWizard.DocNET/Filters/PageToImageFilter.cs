// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using ImageWizard.Attributes;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing.Results;
using SkiaSharp;

namespace ImageWizard.DocNET.Filters;

public partial class PageToImageFilter : DocNETFilter
{
    [Filter]
    public void PageToImage(int pageIndex)
    {
        PageToImage(pageIndex, 1080, 1920);
    }

    [Filter]
    public void PageToImage(int pageIndex, int width, int height)
    {
        IDocReader docReader = DocLib.Instance.GetDocReader(Context.Document.ToByteArray(), new PageDimensions(width, height));
        IPageReader pageReader = docReader.GetPageReader(pageIndex);

        Stream mem = Context.ProcessingContext.StreamPool.GetStream();

        byte[] pixels = pageReader.GetImage();

        var info = new SKImageInfo(
                                pageReader.GetPageWidth(),
                                pageReader.GetPageHeight(),
                                SKColorType.Bgra8888,
                                SKAlphaType.Unpremul);

        using var bitmap = new SKBitmap(info);
        pixels.AsSpan().CopyTo(bitmap.GetPixelSpan());

        using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
        data.SaveTo(mem);

        mem.Seek(0, SeekOrigin.Begin);

        Context.Result = new DataResult(mem, MimeTypes.Png);
    }
}
