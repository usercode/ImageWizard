// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client;

/// <summary>
/// OpenStreetMapExtensions
/// </summary>
public static class OpenStreetMapExtensions
{
    public static Image OpenStreetMap(this ILoader imageUrlBuilder, int z, int x, int y)
    {
        return new Image(imageUrlBuilder.LoadData("openstreetmap", $"{z}/{x}/{y}.png"));
    }
}
