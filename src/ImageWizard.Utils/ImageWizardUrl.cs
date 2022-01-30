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
        public readonly static string Unsafe = "unsafe";

        private readonly static Regex Regex = new Regex($@"^(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([^)]*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private ImageWizardUrl(string signature, string path, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            Signature = signature;
            Path = path;
            LoaderType = loaderType;
            LoaderSource = loaderSource;
            Filters = filters;
        }

        public static ImageWizardUrl CreateUnsafe(string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            return CreateInternal(true, null, null, loaderType, loaderSource, filters);
        }

        public static ImageWizardUrl Create(ISignatureService signatureService, string key, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            return CreateInternal(false, signatureService, key, loaderType, loaderSource, filters);
        }

        private static ImageWizardUrl CreateInternal(bool useUnsafe, ISignatureService? signatureService, string? key, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            string path = $"{string.Join('/', filters)}/{loaderType}/{loaderSource.TrimStart('/')}".TrimStart('/');
            string signature = Unsafe;

            //create signature?
            if (useUnsafe == false)
            {
                if (signatureService == null)
                {
                    throw new ArgumentNullException(nameof(signatureService));
                }

                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                signature = signatureService.Encrypt(key, path);
            }

            ImageWizardUrl url = new ImageWizardUrl(signature, path, loaderType, loaderSource, filters);

            return url;
        }

        public static bool TryParse(string path, out ImageWizardUrl url)
        {
            Match match = Regex.Match(path);

            if (match.Success == false)
            {
                url = new ImageWizardUrl();

                return false;
            }

            string url_signature = match.Groups["signature"].Value;
            string url_path = match.Groups["path"].Value;
            string url_loaderSource = match.Groups["loaderSource"].Value;
            string url_loaderType = match.Groups["loaderType"].Value;
            IList<string> url_filters = match.Groups["filter"].Captures.OfType<Capture>()
                                                                            .Select(x => x.ValueSpan[0..^1].ToString()) //remove "/"
                                                                            .ToList();

            url = new ImageWizardUrl(url_signature, url_path, url_loaderType, url_loaderSource, url_filters);

            return true;
        }

        /// <summary>
        /// Signature
        /// </summary>
        public string Signature { get; }

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
        public IEnumerable<string> Filters { get; }

        /// <summary>
        /// IsUnsafeUrl
        /// </summary>
        public bool IsUnsafeUrl => Signature == Unsafe;

        public override string ToString()
        {
            return $"{Signature}/{Path}";
        }
    }
}
