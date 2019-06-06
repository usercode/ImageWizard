using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImageWizard.Settings;
using ImageWizard.SharedContract;

namespace ImageWizard.Services
{
    /// <summary>
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        public CryptoService(IOptions<ServiceSettings> options)
        {
            Key = Base64Url.FromBase64Url(options.Value.Key);
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

            //return buf.Select(x => x.ToString("x2")).Aggregate(string.Empty, (a, b) => a += b);
        }
    }
}
