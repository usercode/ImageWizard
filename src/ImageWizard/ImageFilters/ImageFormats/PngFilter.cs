using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class PngFilter : FilterBase
    {
        public override string Name => "png";

        public void Execute(FilterContext context)
        {
            context.ImageFormat = new PngFormat();
        }
    }
}
