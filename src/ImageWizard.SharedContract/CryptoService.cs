using ImageWizard.SharedContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.SharedContract
{
    /// <summary>
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        public CryptoService(string key)
        {
            Key = Base64Url.FromBase64Url(key);

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
            if(Key == null)
            {
                throw new Exception("No key available!");
            }

            byte[] buffer = Encoding.Unicode.GetBytes(data);

            HMACSHA1 h = new HMACSHA1(Key);
            byte[] buf = h.ComputeHash(buffer);

            return Base64Url.ToBase64Url(buf);
        }
    }
}
