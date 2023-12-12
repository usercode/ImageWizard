// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using System.Globalization;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public class BlurFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Blur()
    {
        Blur(5);
    }

    [Filter]
    public void Blur(float deviation)
    {
        Context.Filters.Add(new XElement(SvgConstants.SvgNs + "feGaussianBlur", 
                                    new XAttribute("stdDeviation", deviation.ToString(CultureInfo.InvariantCulture))));
    }
}