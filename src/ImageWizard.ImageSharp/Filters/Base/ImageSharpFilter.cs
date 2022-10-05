// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.ImageSharp.Filters;

public abstract class ImageSharpFilter : Filter<ImageSharpFilterContext>
{
    public ImageSharpFilter()
    {

    }

    public override string Namespace => "imagesharp";
}
