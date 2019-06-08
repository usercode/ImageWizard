using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class RotateFilter : FilterBase
    {
        public override string Name => "rotate";

        public void Execute(int rotateValue, FilterContext context)
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

            context.Image.Mutate(m => m.Rotate(rotateMode));
        }
    }
}
