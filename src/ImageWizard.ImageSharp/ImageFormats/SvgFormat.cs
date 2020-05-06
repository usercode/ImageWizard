using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageWizard.Core.Types;
using ImageWizard.ImageFormats.Base;
using SixLabors.ImageSharp;

namespace ImageWizard.ImageFormats
{
    public class SvgFormat : IImageFormat
    {
        public string MimeType => MimeTypes.Svg;

        public void SaveImage(Image image, Stream stream)
        {
            //do nothing....
        }
    }
}
