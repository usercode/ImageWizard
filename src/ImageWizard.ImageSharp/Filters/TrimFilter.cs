// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Attributes;

namespace ImageWizard.ImageSharp.Filters;

public partial class TrimFilter : ImageSharpFilter
{
    [Filter]
    public void Trim()
    {
        //find whitespace

        //int top = 0;
        //int left = 0;
        //int bottom = 0;
        //int right = 0;

        //Task topTask = Task.Run(() =>
        //{
        //    for(int y = 0; y < context.Image.Height; y++)
        //    {
        //        for(int x = 0; x < context.Image.Width; x++)
        //        {
        //            if(context.Image[x,y] != Rgba32.White)
        //            {
        //                top = y;
        //                break;
        //            }
        //        }
        //    }
        //});

        //Task bottomTask = Task.Run(() =>
        //{
        //    for (int y = context.Image.Height; y >= 0; y--)
        //    {
        //        for (int x = 0; x < context.Image.Width; x++)
        //        {
        //            if (context.Image[x, y] != Rgba32.White)
        //            {
        //                bottom = y;
        //                break;
        //            }
        //        }
        //    }
        //});
        
        //Task.WhenAll(topTask).Wait();
        
        //context.Image.Mutate(m => m.Crop(new Rectangle(x, y, width, height)));
    }
}
