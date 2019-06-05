using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImageWizard.Settings;

namespace ImageWizard.Services
{
    /// <summary>
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        public CryptoService(IOptions<ServiceSettings> options)
        {
            Key = FromBase64Url(options.Value.Key);
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

            return ToBase64Url(buf);

            //return buf.Select(x => x.ToString("x2")).Aggregate(string.Empty, (a, b) => a += b);
        }

        /// <summary>
        /// FromBase64Url
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] FromBase64Url(string value)
        {
            return Convert.FromBase64String(
                                            value
                                                .Replace('-', '+')
                                                .Replace('_', '/')
                );
        }

        /// <summary>
        /// ToBase64Url
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ToBase64Url(byte[] data)
        {
            return Convert.ToBase64String(data)
                                .Replace('+', '-')
                                .Replace('/', '_')
                                .Substring(0, 27); //remove padding '='
        }
    }
}
