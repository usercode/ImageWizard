using ImageWizard.Core.ImageFilters.Base;
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
    public class ResizeFilter : ImageFilter
    {
        [Filter]
        public void Resize([DPR]int size)
        {
            Resize(size, size);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height)
        {
            Resize(width, height, Utils.FilterTypes.ResizeMode.Max);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, Utils.FilterTypes.ResizeMode mode)
        {
            Resize(width, height, mode, Utils.FilterTypes.AnchorPositionMode.Center);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, Utils.FilterTypes.ResizeMode mode, Utils.FilterTypes.AnchorPositionMode position)
        {
            ResizeMode mode2 = mode switch
            {
                Utils.FilterTypes.ResizeMode.Max => ResizeMode.Max,
                Utils.FilterTypes.ResizeMode.Min => ResizeMode.Min,
                Utils.FilterTypes.ResizeMode.Stretch => ResizeMode.Stretch,
                Utils.FilterTypes.ResizeMode.Pad => ResizeMode.Pad,
                Utils.FilterTypes.ResizeMode.Crop => ResizeMode.Crop,
                _ => throw new Exception(),
            };

            AnchorPositionMode anchorPositionMode = position switch
            {
                Utils.FilterTypes.AnchorPositionMode.Bottom => AnchorPositionMode.Bottom,
                Utils.FilterTypes.AnchorPositionMode.BottomLeft => AnchorPositionMode.BottomLeft,
                Utils.FilterTypes.AnchorPositionMode.BottomRight => AnchorPositionMode.BottomRight,
                Utils.FilterTypes.AnchorPositionMode.Center => AnchorPositionMode.Center,
                Utils.FilterTypes.AnchorPositionMode.Left => AnchorPositionMode.Left,
                Utils.FilterTypes.AnchorPositionMode.Right => AnchorPositionMode.Right,
                Utils.FilterTypes.AnchorPositionMode.Top => AnchorPositionMode.Top,
                Utils.FilterTypes.AnchorPositionMode.TopLeft => AnchorPositionMode.TopLeft,
                Utils.FilterTypes.AnchorPositionMode.TopRight => AnchorPositionMode.TopRight,
                _ => throw new Exception(),
            };

            //prevent upscaling
            if (width > Context.Image.Width)
            {
                width = Context.Image.Width;
            }

            if (height > Context.Image.Height)
            {
                height = Context.Image.Height;
            }

            Context.Image.Mutate(m => m.Resize(new ResizeOptions()
            {
                Position = anchorPositionMode,
                Mode = mode2,
                Size = new Size(width, height)
            }));
        }
    }
}
