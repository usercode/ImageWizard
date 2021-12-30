using ImageWizard.Core.ImageFilters.Base.Attributes;
using ImageWizard.Core.Types;
using ImageWizard.Filters;
using ImageWizard.Processing.Results;
using ImageWizard.Services.Types;
using Svg;
using Svg.FilterEffects;
using Svg.Transforms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ImageWizard.SvgNet.Filters
{
    public class ImageFormatFilter : ImageWizard.Filters.SvgFilter
    {
        [Filter]
        public void Png()
        {
            SaveToImage(ImageFormat.Png);
        }

        [Filter]
        public void Jpg()
        {
            SaveToImage(ImageFormat.Jpeg);
        }

        [Filter]
        public void Gif()
        {
            SaveToImage(ImageFormat.Gif);
        }

        [Filter]
        public void Bmp()
        {
            SaveToImage(ImageFormat.Bmp);
        }

        private void SaveToImage(ImageFormat format)
        {
            var bitmap = Context.Image.Draw();

            MemoryStream mem = new MemoryStream();

            bitmap.Save(mem, format);

            string mimeType;

            if(format == ImageFormat.Png)
            {
                mimeType = MimeTypes.Png;
            }
            else if(format == ImageFormat.Jpeg)
            {
                mimeType = MimeTypes.Jpeg;
            }
            else if(format == ImageFormat.Gif)
            {
                mimeType = MimeTypes.Gif;
            }
            else if (format == ImageFormat.Bmp)
            {
                mimeType = MimeTypes.Bmp;
            }
            else
            {
                throw new Exception();
            }

            Context.Result = new ImageResult(mem.ToArray(), mimeType);
        }

    }
}
