using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Core.Middlewares
{
    /// <summary>
    /// ImageWizardUrlRegex
    /// </summary>
    public class ImageWizardUrlRegex
    {
        static ImageWizardUrlRegex()
        {
            const string signature = @"[a-z0-9-_]+";
            const string filter = @"[a-z]+\([^)]*\)";
            const string loaderType = @"[a-z]+";
            const string loaderSource = @".*";

            Url = new Regex($@"^(?<signature>{signature})/(?<path>(?<filter>{filter}/)*(?<loaderType>{loaderType})/(?<loaderSource>{loaderSource}))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static Regex Url { get; }



    }
}
