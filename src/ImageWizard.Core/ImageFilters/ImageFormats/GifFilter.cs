using ImageWizard.ImageFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters.ImageFormats
{
    public class GifFilter : FilterBase
    {
        public override string Name => "gif";

        public void Execute(FilterContext context)
        {
            context.ImageFormat = new GifFormat();
        }
    }
}
