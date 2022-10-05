// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.ImageSharp;

public class ImageFormatHelper
{
    public static IImageFormat FirstOrDefault(string mimeType)
    {
        return FirstOrDefault(new[] { mimeType });
    }

    public static IImageFormat FirstOrDefault(IEnumerable<string> mimeTypes)
    {
        foreach (string mimeType in mimeTypes)
        {
            IImageFormat imageFormat = mimeType switch
            {
                MimeTypes.WebP => new WebPFormat(),
                MimeTypes.Jpeg => new JpegFormat(),
                MimeTypes.Png => new PngFormat(),
                MimeTypes.Gif => new GifFormat(),                    
                MimeTypes.Tga => new TgaFormat(),
                MimeTypes.Bmp => new BmpFormat(),
                _ => null,
            };

            if (imageFormat != null)
            {
                return imageFormat;
            }
        }

        return null;
    }
}
