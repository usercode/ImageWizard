// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Loaders;
using Microsoft.Extensions.Options;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace ImageWizard.ImageSharp;

public partial class PlaceholderLoader : Loader<PlaceholderOptions>
{
    public PlaceholderLoader(IOptions<PlaceholderOptions> options)
        : base(options)
    {
    }

    [GeneratedRegex("^(?<width>[0-9]+)x(?<height>[0-9]+)$")]
    private partial Regex RegexSource();

    public override async Task<LoaderResult> GetAsync(string source, ICachedData? existingCachedData)
    {
        Match match = RegexSource().Match(source);

        if (match.Success == false)
        {
            return LoaderResult.Failed();
        }

        int width = int.Parse(match.Groups["width"].Value);
        int height = int.Parse(match.Groups["height"].Value);

        Image image = new Image<Rgba32>(width, height, Rgba32.ParseHex(Options.Value.BackgroundColor));

        string text = $"{width}x{height}";

        var font = SystemFonts.Get(Options.Value.FontName).CreateFont(Options.Value.FontSize, FontStyle.Regular);
        var textOptions = new TextOptions(font) { Dpi = 72, KerningMode = KerningMode.Standard };
        var rect = TextMeasurer.MeasureSize(text, textOptions);

        image.Mutate(x => x.DrawText(
                             $"{image.Width} x {image.Height}",
                            font,
                            new Color(Rgba32.ParseHex(Options.Value.FontColor)),
                            new PointF((image.Width / 2) - (rect.Width / 2), (image.Height / 2) - (rect.Height / 2) )));

        MemoryStream mem = new MemoryStream();
        
        await image.SaveAsPngAsync(mem);
        
        mem.Seek(0, SeekOrigin.Begin);

        return LoaderResult.Success(new OriginalData(MimeTypes.Png, mem));
    }
}
