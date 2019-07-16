using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    public class ImageMetadata : IImageMetadata
    {
        public ImageMetadata()
        {
        }

        public string Signature { get; set; }
        public string MimeType { get; set; }
        public string ImageSource { get; set; }

        public double? DPR { get; set; }
    }
}
