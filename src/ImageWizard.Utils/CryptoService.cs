using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Utils
{
    /// <summary>
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        public CryptoService(string key)
        {
            try
            {
                Key = WebEncoders.Base64UrlDecode(key);
            }
            catch(Exception ex)
            {
                throw new Exception("No valid key: " + ex.Message);
            }
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

            byte[] buffer = Encoding.UTF8.GetBytes(data);

            HMACSHA256 h = new HMACSHA256(Key);
            byte[] buf = h.ComputeHash(buffer);

            return WebEncoders.Base64UrlEncode(buf);
        }
    }
}
