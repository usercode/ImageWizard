using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class JpgFilter : FilterBase
    {
        public override string Name => "jpg";

        public void Execute(FilterContext context)
        {
            context.ImageFormat = new JpegFormat();
        }

        public void Execute(int quality, FilterContext context)
        {
            context.ImageFormat = new JpegFormat() { Quality = quality };
        }
    }
}
