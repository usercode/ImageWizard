using ImageWizard.Utils;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ImageWizard
{
    /// <summary>
    /// ImageWizardOptions
    /// </summary>
    public class ImageWizardOptions : ImageWizardBaseOptions
    {
        public ImageWizardOptions()
        {
            UseETag = true;
            UseClintHints = false;
            AllowUnsafeUrl = false;
            UseAcceptHeader = false;
            Key = string.Empty;

            AllowedDPR = ImageWizardDefaults.AllowedDPR;

            CacheControl = new CacheControl();
        }

        /// <summary>
        /// CacheControl
        /// </summary>
        public CacheControl CacheControl { get; }

        /// <summary>
        /// Allows unsafe url.
        /// </summary>
        public bool AllowUnsafeUrl { get; set; }

        /// <summary>
        /// Selects automatically the compatible mime type by request header. (Default: false)
        /// </summary>
        public bool UseAcceptHeader { get; set; }

        /// <summary>
        /// Use ETag. (Default: true)
        /// </summary>
        public bool UseETag { get; set; }

        /// <summary>
        /// Use clint hints. (Default: false)
        /// </summary>
        public bool UseClintHints { get; set; }

        

        /// <summary>
        /// Allowed DPR values. (Default: 1.0, 1.5, 2.0, 3.0)
        /// </summary>
        public double[] AllowedDPR { get; set; }
      
        /// <summary>
        /// Generates random 64 byte key.
        /// </summary>
        public void GenerateRandomKey()
        {
            //generate random key
            byte[] keyBuffer = new byte[64];
            RandomNumberGenerator.Create().GetBytes(keyBuffer);

            Key = WebEncoders.Base64UrlEncode(keyBuffer);
        }
    }
}
