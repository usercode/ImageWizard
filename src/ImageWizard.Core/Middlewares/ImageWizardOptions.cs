using ImageWizard.Core.Settings;
using ImageWizard.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ImageWizard.Settings
{
    /// <summary>
    /// ImageWizardOptions
    /// </summary>
    public class ImageWizardOptions
    {
        public ImageWizardOptions()
        {
            BasePath = ImageWizardConstants.DefaultBasePath;
            UseETag = true;
            UseClintHints = false;
            AllowUnsafeUrl = false;
            UseAcceptHeader = false;
            Key = null;

            AllowedDPR = new[] { 1.0, 1.5, 2.0, 3.0, 4.0 };

            CacheControl = new CacheControl();
        }

        /// <summary>
        /// BasePath
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// CacheControl
        /// </summary>
        public CacheControl CacheControl { get; }

        /// <summary>
        /// AllowUnsafeUrl
        /// </summary>
        public bool AllowUnsafeUrl { get; set; }

        /// <summary>
        /// AutoFormat
        /// </summary>
        public bool UseAcceptHeader { get; set; }

        /// <summary>
        /// UseETag
        /// </summary>
        public bool UseETag { get; set; }

        /// <summary>
        /// UseClintHints
        /// </summary>
        public bool UseClintHints { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// AllowedDPR
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
