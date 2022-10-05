// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Filters;

public abstract class SvgFilter : Filter<SvgFilterContext>
{
    public SvgFilter()
    {

    }

    public override string Namespace => "svgnet";

}
