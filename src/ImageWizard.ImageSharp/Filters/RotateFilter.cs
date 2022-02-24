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
    public class RotateFilter : ImageSharpFilter
    {
        [Filter]
        public void Rotate(float angle)
        {
            RotateMode rotateMode;

            switch (angle)
            {
                case 90:
                    rotateMode = RotateMode.Rotate90;
                    break;

                case 180:
                    rotateMode = RotateMode.Rotate180;
                    break;

                case 270:
                    rotateMode = RotateMode.Rotate270;
                    break;

                default:
                    throw new Exception("angle is not supported: " + angle);
            }

            Context.Image.Mutate(m => m.Rotate(rotateMode));
        }
    }
}
