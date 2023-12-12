// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using ImageWizard.Attributes;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Processing.Results;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageWizard.DocNET.Filters;

public class PageToImageFilter : DocNETFilter
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

        Image<Bgra32> image = Image.LoadPixelData<Bgra32>(pageReader.GetImage(), pageReader.GetPageWidth(), pageReader.GetPageHeight());

        image.SaveAsPng(mem);

        mem.Seek(0, SeekOrigin.Begin);

        Context.Result = new DataResult(mem, MimeTypes.Png);
    }
}
