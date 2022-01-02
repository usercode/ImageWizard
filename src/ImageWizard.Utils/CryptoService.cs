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
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string key, string data)
        {
            byte[] keyBytes;

            try
            {
                keyBytes = WebEncoders.Base64UrlDecode(key);
            }
            catch (Exception ex)
            {
                throw new Exception("No valid key: " + ex.Message);
            }
            
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            byte[] buf = HMACSHA256.HashData(keyBytes, buffer);

            return WebEncoders.Base64UrlEncode(buf);
        }
    }
}
