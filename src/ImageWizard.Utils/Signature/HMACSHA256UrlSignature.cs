using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils
{
    /// <summary>
    /// HMACSHA256UrlSignature
    /// </summary>
    public class HMACSHA256UrlSignature : IUrlSignature
    {
        public HMACSHA256UrlSignature()
        {
            IncludeHost = false;
            IsCaseInsensitive = false;
        }

        public HMACSHA256UrlSignature(bool includeHost, bool isCaseInsensitive)
        {
            IncludeHost = includeHost;
            IsCaseInsensitive = isCaseInsensitive;
        }

        /// <summary>
        /// Signature depend on remote hostname? (Default: false)
        /// </summary>
        public virtual bool IncludeHost { get; }

        /// <summary>
        /// CaseSensitive
        /// </summary>
        public virtual bool IsCaseInsensitive { get; }

        /// <summary>
        /// Selects part of the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetUrlValue(ImageWizardUrl url)
        {
            return url.Path;
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt(byte[] key, ImageWizardRequest request)
        {
            //build data string
            StringBuilder builder = new StringBuilder();

            if (IncludeHost == true)
            {
                //add host
                builder.Append(request.Host.Value);
                builder.Append('/');
            }

            //add url
            builder.Append(GetUrlValue(request.Url));

            string data = builder.ToString();

            if (IsCaseInsensitive == true)
            {
                data = data.ToLowerInvariant();
            }

            byte[] inputBuffer = Encoding.UTF8.GetBytes(data);

            //HMACSHA256 => 256 / 8 = 32
            Span<byte> hashBufferSpan = stackalloc byte[32];

            //create hash
            HMACSHA256.HashData(key, inputBuffer, hashBufferSpan);

            //convert to Base64Url
            return WebEncoders.Base64UrlEncode(hashBufferSpan);
        }
    }
}
