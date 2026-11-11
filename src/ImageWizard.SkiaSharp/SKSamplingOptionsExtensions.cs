// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using SkiaSharp;

namespace ImageWizard.SkiaSharp;

internal static class SKSamplingOptionsExtensions
{
    extension(SKSamplingOptions)
    {
        public static SKSamplingOptions Default2 => new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);
    }
}
