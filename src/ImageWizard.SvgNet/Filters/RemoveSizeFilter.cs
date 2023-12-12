// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using ImageWizard.Filters;
using System.Xml.Linq;

namespace ImageWizard.SvgNet.Filters;

public class RemoveSizeFilter : SvgFilter
{
    [Filter]
    public void RemoveSize()
    {
        XAttribute? width = Context.Root.Attribute("width");

        if (width != null)
        {
            width.Remove();
        }

        XAttribute? height = Context.Root.Attribute("height");

        if (height != null)
        {
            height.Remove();
        }
    }
}
