using ImageWizard.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Types
{
    public class CachedImage
    {
        public CachedImage()
        {

        }

        public byte[] Data { get; set; }

        public IImageMetadata Metadata { get; set; }
    }
}
