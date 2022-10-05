// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using Svg.Transforms;

namespace ImageWizard.SvgNet.Filters;

public class RotateFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Rotate(float angle)
    {
        Context.Image.Transforms.Add(new SvgRotate(angle));
    }

}
