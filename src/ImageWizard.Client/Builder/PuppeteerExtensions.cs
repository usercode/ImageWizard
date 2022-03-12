// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// PuppeteerExtensions
    /// </summary>
    public static class PuppeteerExtensions
    {
        public static Image Screenshot(this ILoader imageUrlBuilder, string source)
        {
            return new Image(imageUrlBuilder.LoadData("screenshot", source));
        }
    }
}
