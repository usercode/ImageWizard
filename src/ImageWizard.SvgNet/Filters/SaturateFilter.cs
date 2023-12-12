// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using System.Globalization;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public class SaturateFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Saturate(float value)
    {
        Context.Filters.Add(new XElement(SvgConstants.SvgNs + "feColorMatrix",
                                    new XAttribute("type", "saturate"),
                                    new XAttribute("values", value.ToString(CultureInfo.InvariantCulture))));
    }
}
