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
    /// CryptoService
    /// </summary>
    public class CryptoService
    {
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encrypt(string key, string input)
        {
            byte[] keyBuffer;

            try
            {
                keyBuffer = WebEncoders.Base64UrlDecode(key);
            }
            catch (Exception ex)
            {
                throw new Exception("No valid key: " + ex.Message);
            }

            //convert data string to buffer
            byte[] inputBuffer = Encoding.UTF8.GetBytes(input);

            //HMACSHA256 => 256 / 8 = 32
            Span<byte> hashBufferSpan = stackalloc byte[32];

            //create hash
            HMACSHA256.HashData(keyBuffer, inputBuffer, hashBufferSpan);

            //convert to Base64Url
            return WebEncoders.Base64UrlEncode(hashBufferSpan);
        }
    }
}
