using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class BmpFilter : FilterBase
    {
        public override string Name => "bmp";

        public void Execute(FilterContext context)
        {
            context.ImageFormat = new BmpFormat();
        }
    }
}
