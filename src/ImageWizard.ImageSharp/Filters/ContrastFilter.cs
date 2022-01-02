﻿using ImageWizard.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class ContrastFilter : ImageSharpFilter
    {
        [Filter]
        public void Contrast(float value)
        {
            Context.Image.Mutate(m => m.Contrast(value));
        }
    }
}
