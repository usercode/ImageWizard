using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImageWizard.Client
{
    static class BuilderExtensions
    {
        public static string ToUrlString(this double value)
        {
            return value.ToString("0.0########", CultureInfo.InvariantCulture);
        }

        public static string ToUrlString(this Enum value)
        {
            return value.ToString().ToLower();
        }
    }
}
