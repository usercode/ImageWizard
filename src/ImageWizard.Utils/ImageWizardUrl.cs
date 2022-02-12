using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard
{
    /// <summary>
    /// ImageWizardUrl
    /// </summary>
    public readonly struct ImageWizardUrl
    {
        private readonly static Regex Regex = new Regex($@"^(?<path>(?<filter>[a-z]+\([^)]*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ImageWizardUrl(string loaderType, string loaderSource, string[] filters)
        {
            LoaderType = loaderType;
            LoaderSource = loaderSource.TrimStart('/');
            Filters = filters;

            if (Filters.Length > 0)
            {
                Path = $"{string.Join('/', Filters)}/{LoaderType}/{LoaderSource}";
            }
            else
            {
                Path = $"{LoaderType}/{LoaderSource}";
            }
        }

        private ImageWizardUrl(string path, string loaderType, string loaderSource, string[] filters)
        {
            LoaderType = loaderType;
            LoaderSource = loaderSource;
            Filters = filters;
            Path = path;
        }

        public static bool TryParse(string path, out ImageWizardUrl url)
        {
            Match match = Regex.Match(path);

            if (match.Success == false)
            {
                url = new ImageWizardUrl();

                return false;
            }

            string url_path = match.Groups["path"].Value;
            string url_loaderSource = match.Groups["loaderSource"].Value;
            string url_loaderType = match.Groups["loaderType"].Value;
            string[] url_filters = match.Groups["filter"].Captures.OfType<Capture>()
                                                                            .Select(x => x.ValueSpan[0..^1].ToString()) //remove "/"
                                                                            .ToArray();

            url = new ImageWizardUrl(url_path, url_loaderType, url_loaderSource, url_filters);

            return true;
        }

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// LoaderType
        /// </summary>
        public string LoaderType { get; }

        /// <summary>
        /// LoaderSource
        /// </summary>
        public string LoaderSource { get; }

        /// <summary>
        /// Filters
        /// </summary>
        public string[] Filters { get; }

        public override string ToString()
        {
            return Path;
        }
    }
}
