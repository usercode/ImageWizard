using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Utils
{
    public class ImageWizardDefaults
    {
        public readonly static string Unsafe = "unsafe";
        public readonly static string BasePath = "/image";
        public readonly static double[] AllowedDPR = new[] { 1.0, 1.5, 2.0, 3.0 };
    }
}
