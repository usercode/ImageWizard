using ImageWizard.Client.Builder;
using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;

namespace ImageWizard
{
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
    }
}
