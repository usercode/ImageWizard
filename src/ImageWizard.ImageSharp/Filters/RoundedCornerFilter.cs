using ImageWizard.Core.ImageFilters.Base.Attributes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageWizard.ImageSharp.Filters
{
    public class RoundedCornerFilter : ImageFilter
    {
        [Filter]
        public void RoundedCorner(float value)
        {
            Context.Image.Mutate(x => ApplyRoundedCorners(x, value));
        }

        private IImageProcessingContext ApplyRoundedCorners(IImageProcessingContext ctx, float cornerRadius)
        {
            Size size = ctx.GetCurrentSize();

            int s = Math.Min(size.Width, size.Height) / 2;

            IPathCollection corners = BuildCorners(size.Width, size.Height, s * cornerRadius);

            var graphicOptions = new GraphicsOptions(true)
            {
                AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // enforces that any part of this shape that has color is punched out of the background
            };
            // mutating in here as we already have a cloned original
            // use any color (not Transparent), so the corners will be clipped
            return ctx.Fill(graphicOptions, Rgba32.LimeGreen, corners);
        }

        private IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
        {
            // first create a square
            var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

            // then cut out of the square a circle so we are left with a corner
            IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

            // corner is now a corner shape positions top left
            //lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

            float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
            float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

            // move it across the width of the image - the width of the shape
            IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
            IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
            IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

            return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
        }
    }
}
