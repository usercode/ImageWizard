// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public partial class GrayscaleFilter : ImageWizard.Filters.SvgFilter
{
    [Filter]
    public void Grayscale()
    {
        Context.Filters.Add(new XElement(SvgConstants.SvgNs + "feColorMatrix", 
                                    new XAttribute("type", "matrix"), 
                                    new XAttribute("values", "0.21 0.72 0.07 0 0 0.21 0.72 0.07 0 0 0.21 0.72 0.07 0 0 0 0 0 1 0")));
    }

}
