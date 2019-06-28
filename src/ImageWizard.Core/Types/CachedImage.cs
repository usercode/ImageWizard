using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Services.Types
{
    public class CachedImage
    {
        public byte[] Data { get; set; }
        public ImageMetadata Metadata { get; set; }
    }
}
