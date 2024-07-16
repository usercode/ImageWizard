// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;
using SixLabors.ImageSharp.Processing;

namespace ImageWizard.ImageSharp.Filters;

public partial class CropFilter : ImageSharpFilter
{
    [Filter]
    public void Crop(double width, double height)
    {
        Crop(0, 0, width, height);
    }

    [Filter]
    public void Crop(double x, double y, double width, double height)
    {
        Crop(
            (int)(x * Context.Image.Width),
            (int)(y * Context.Image.Height),
            (int)(width * Context.Image.Width),
            (int)(height * Context.Image.Height));
    }

    [Filter]
    public void Crop(int width, int height)
    {
        Crop(0, 0, width, height);
    }

    [Filter]
    public void Crop(int x, int y, int width, int height)
    {
       Context.Image.Mutate(m => m.Crop(new SixLabors.ImageSharp.Rectangle(x, y, width, height)));
    }
}
