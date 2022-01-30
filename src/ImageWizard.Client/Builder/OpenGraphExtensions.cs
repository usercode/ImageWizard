using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// OpenGraphExtensions
    /// </summary>
    public static class OpenGraphExtensions
    {
        public static Image OpenGraph(this ILoader imageUrlBuilder, string source)
        {
            return new Image(imageUrlBuilder.LoadData("opengraph", source));
        }
    }
}
