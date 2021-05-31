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
    /// ImageWizardUrl
    /// </summary>
    public class ImageWizardUrl
    {
        public ImageWizardUrl(string signature, string path, string loaderType, string loaderSource, IList<string> filters)
        {
            Signature = signature;
            Path = path;
            LoaderType = loaderType;
            LoaderSource = loaderSource;
            Filters = filters;            
        }

        public static bool TryParsePath(string path, out ImageWizardUrl? url)
        {
            Match match = ImageWizardUrlRegex.Url.Match(path);

            if (match.Success == false)
            {
                url = null;

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
        public string Signature { get; private set; }

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
        public IList<string> Filters { get; }

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

        /// <summary>
        /// CreateSignature
        /// </summary>
        /// <param name="key"></param>
        public void CreateSignature(string key)
        {
            Signature = new CryptoService(key).Encrypt(Path);
        }

    }
}
