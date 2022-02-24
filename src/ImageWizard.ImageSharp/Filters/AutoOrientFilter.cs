// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

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
    public class AutoOrientFilter : ImageSharpFilter
    {
        [Filter]
        public void AutoOrient()
        {
            Context.Image.Mutate(m => m.AutoOrient());
        }
    }
}
