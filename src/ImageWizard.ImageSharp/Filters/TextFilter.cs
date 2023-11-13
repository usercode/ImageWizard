// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public class TextFilter : ImageSharpFilter
{
    [Filter]
    public void DrawText(float x = 0, float y = 0, string text = "", int size = 24, string font = "Arial")
    {
        Context.Image.Mutate(m =>
         {
             m.DrawText(
                 text,
                 new Font(SystemFonts.Get(font), size),
                 Color.Black,
                 new PointF(Context.Image.Width * x, Context.Image.Height * y));
         });
    }
}
