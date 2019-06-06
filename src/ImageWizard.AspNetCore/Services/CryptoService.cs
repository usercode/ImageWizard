using ImageWizard.SharedContract;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.AspNetCore.Services
{
    /// <summary>
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        public CryptoService(IOptions<ImageWizardSettings> settings)
        {
            Key = Base64Url.FromBase64Url(settings.Value.Key);
        }

        /// <summary>
        /// ServiceSettings
        /// </summary>
        public byte[] Key { get; }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(data);

            HMACSHA1 h = new HMACSHA1(Key);
            byte[] buf = h.ComputeHash(buffer);

            return Base64Url.ToBase64Url(buf);
        }
    }
}
