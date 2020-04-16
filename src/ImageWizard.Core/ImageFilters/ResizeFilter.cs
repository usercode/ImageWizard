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

namespace ImageWizard.Filters
{
    public class ResizeFilter : FilterBase
    {
        [Filter]
        public void Resize([DPR]int size, FilterContext context)
        {
            Resize(size, size, context);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, FilterContext context)
        {
            Resize(width, height, SharedContract.FilterTypes.ResizeMode.Max, context);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, SharedContract.FilterTypes.ResizeMode mode, FilterContext context)
        {
            Resize(width, height, mode, SharedContract.FilterTypes.AnchorPositionMode.Center, context);
        }

        [Filter]
        public void Resize([DPR]int width, [DPR]int height, SharedContract.FilterTypes.ResizeMode mode, SharedContract.FilterTypes.AnchorPositionMode position, FilterContext context)
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
            if (width > context.Image.Width)
            {
                width = context.Image.Width;
            }

            if (height > context.Image.Height)
            {
                height = context.Image.Height;
            }

            context.Image.Mutate(m => m.Resize(new ResizeOptions()
            {
                Position = anchorPositionMode,
                Mode = mode2,
                Size = new Size(width, height)
            }));
        }
    }
}
