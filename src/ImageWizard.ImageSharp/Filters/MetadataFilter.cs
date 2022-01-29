using ImageWizard.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class MetadataFilter : ImageSharpFilter
    {
        [Filter]
        public void RemoveExif()
        {
            Context.Image.Metadata.ExifProfile = null;
        }

        [Filter]
        public void RemoveIcc()
        {
            Context.Image.Metadata.IccProfile = null;
        }

        [Filter]
        public void RemoveIptc()
        {
            Context.Image.Metadata.IptcProfile = null;
        }

        [Filter]
        public void RemoveMetadata()
        {
            RemoveExif();
            RemoveIcc();
            RemoveIptc();
        }
    }
}
