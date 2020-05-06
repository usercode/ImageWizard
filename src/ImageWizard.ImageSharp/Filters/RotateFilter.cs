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
    public class RotateFilter : ImageFilter
    {
        [Filter]
        public void Rotate(int rotateValue)
        {
            RotateMode rotateMode;

            switch (rotateValue)
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
                    throw new Exception();
            }

            Context.Image.Mutate(m => m.Rotate(rotateMode));
        }
    }
}
