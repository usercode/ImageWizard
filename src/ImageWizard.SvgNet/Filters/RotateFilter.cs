// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using System.Globalization;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public partial class RotateFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Rotate(float angle)
    {
        Context.Root.Add(new XAttribute("transform", 
                                    $"rotate({angle.ToString(CultureInfo.InvariantCulture)})"));
    }

}
