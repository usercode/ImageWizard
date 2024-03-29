﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Client;

/// <summary>
/// FilterExtensions
/// </summary>
public static class FilterExtensions
{
    public static Image AsImage(this IFilter filter)
    {
        return new Image(filter);
    }

    public static Svg AsSvg(this IFilter filter)
    {
        return new Svg(filter);
    }

    public static Pdf AsPdf(this IFilter filter)
    {
        return new Pdf(filter);
    }

    public static Video AsVideo(this IFilter filter)
    {
        return new Video(filter);
    }
}
