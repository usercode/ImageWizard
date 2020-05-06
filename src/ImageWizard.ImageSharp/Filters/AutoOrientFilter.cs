using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class AutoOrientFilter : ImageFilter
    {
        [Filter]
        public void AutoOrient()
        {
            Context.Image.Mutate(m => m.AutoOrient());
        }
    }
}
