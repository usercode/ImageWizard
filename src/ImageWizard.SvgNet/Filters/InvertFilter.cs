// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public partial class InvertFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Invert()
    {
        Context.Filters.Add(new XElement(SvgConstants.SvgNs + "feColorMatrix",
                                    new XAttribute("type", "matrix"),
                                    new XAttribute("values", "-1 0 0 0 1 0 -1 0 0 1 0 0 -1 0 1 0 0 0 1 0")));
    }

}
