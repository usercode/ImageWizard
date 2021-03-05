using Docnet.Core;
using Docnet.Core.Models;
using Docnet.Core.Readers;
using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.Types;
using ImageWizard.DocNET.Filters.Base;
using ImageWizard.Services.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.DocNET.Filters
{
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
            IDocReader docReader = DocLib.Instance.GetDocReader(Context.Document, new PageDimensions(width, height));
            IPageReader pageReader = docReader.GetPageReader(pageIndex);

            byte[] imageData = pageReader.GetImage();

            MemoryStream mem = new MemoryStream();
            using (Image<Bgra32> image = new Image<Bgra32>(pageReader.GetPageWidth(), pageReader.GetPageHeight()))
            {
                image.TryGetSinglePixelSpan(out Span<Bgra32> span);

                Span<Bgra32> imageData2 = MemoryMarshal.Cast<byte, Bgra32>(imageData);
                imageData2.CopyTo(span);

                image.SaveAsPng(mem);
            }

            Context.Result = new ImageResult(mem.ToArray(), MimeTypes.Png);
        }
    }
}
