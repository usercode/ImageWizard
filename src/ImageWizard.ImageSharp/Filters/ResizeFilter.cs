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
            Resize(width, height, SharedContract.FilterTypes.ResizeMode.Max);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, SharedContract.FilterTypes.ResizeMode mode)
        {
            Resize(width, height, mode, SharedContract.FilterTypes.AnchorPositionMode.Center);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, SharedContract.FilterTypes.ResizeMode mode, SharedContract.FilterTypes.AnchorPositionMode position)
        {
            ResizeMode mode2 = mode switch
            {
                SharedContract.FilterTypes.ResizeMode.Max => ResizeMode.Max,
                SharedContract.FilterTypes.ResizeMode.Min => ResizeMode.Min,
                SharedContract.FilterTypes.ResizeMode.Stretch => ResizeMode.Stretch,
                SharedContract.FilterTypes.ResizeMode.Pad => ResizeMode.Pad,
                SharedContract.FilterTypes.ResizeMode.Crop => ResizeMode.Crop,
                _ => throw new Exception(),
            };

            AnchorPositionMode anchorPositionMode = position switch
            {
                SharedContract.FilterTypes.AnchorPositionMode.Bottom => AnchorPositionMode.Bottom,
                SharedContract.FilterTypes.AnchorPositionMode.BottomLeft => AnchorPositionMode.BottomLeft,
                SharedContract.FilterTypes.AnchorPositionMode.BottomRight => AnchorPositionMode.BottomRight,
                SharedContract.FilterTypes.AnchorPositionMode.Center => AnchorPositionMode.Center,
                SharedContract.FilterTypes.AnchorPositionMode.Left => AnchorPositionMode.Left,
                SharedContract.FilterTypes.AnchorPositionMode.Right => AnchorPositionMode.Right,
                SharedContract.FilterTypes.AnchorPositionMode.Top => AnchorPositionMode.Top,
                SharedContract.FilterTypes.AnchorPositionMode.TopLeft => AnchorPositionMode.TopLeft,
                SharedContract.FilterTypes.AnchorPositionMode.TopRight => AnchorPositionMode.TopRight,
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
