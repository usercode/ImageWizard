// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System.Globalization;

namespace ImageWizard.Client;

static class BuilderExtensions
{
    public static string ToUrlString(this double value)
    {
        return value.ToString("0.0########", CultureInfo.InvariantCulture);
    }

    public static string ToUrlString(this Enum value)
    {
        return value.ToString().ToLowerInvariant();
    }
}
