using ImageWizard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Core
{
    /// <summary>
    /// ImageWizardUrl
    /// </summary>
    public readonly struct ImageWizardUrl
    {
        public const string Unsafe = "unsafe";

        private readonly static Regex Regex = new Regex($@"^(?<signature>[a-z0-9-_]+)/(?<path>(?<filter>[a-z]+\([^)]*\)/)*(?<loaderType>[a-z]+)/(?<loaderSource>.*))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ImageWizardUrl(string signature, string path, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            Signature = signature;
            Path = path;
            LoaderType = loaderType;
            LoaderSource = loaderSource;
            Filters = filters;
        }

        public static ImageWizardUrl CreateUnsafe(string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            return CreateInternal(true, null, loaderType, loaderSource, filters);
        }

        public static ImageWizardUrl Create(string key, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            return CreateInternal(false, key, loaderType, loaderSource, filters);
        }

        private static ImageWizardUrl CreateInternal(bool isUnsafe, string? key, string loaderType, string loaderSource, IEnumerable<string> filters)
        {
            string signature = Unsafe;
            string path = $"{string.Join('/', filters)}/{loaderType}/{loaderSource.TrimStart('/')}".TrimStart('/');

            if (isUnsafe == false)
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                signature = new CryptoService(key).Encrypt(path);
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
                                                                            .Select(x => x.Value)
                                                                            .Select(x => x[0..^1]) //remove "/"
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
        public bool IsUnsafeUrl => Signature == "unsafe";

        /// <summary>
        /// IsSignatureValid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSignatureValid(string key)
        {
            string signature = new CryptoService(key).Encrypt(Path);

            return signature == Signature;
        }
    }
}
