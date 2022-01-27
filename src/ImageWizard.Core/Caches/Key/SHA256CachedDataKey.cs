using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    public class SHA256CachedDataKey : ICachedDataKey
    {
        public string Create(string input)
        {
            Span<byte> keyBufferSpan = stackalloc byte[32];

            SHA256.HashData(Encoding.UTF8.GetBytes(input), keyBufferSpan);

            string key = Convert.ToHexString(keyBufferSpan);

            return key;
        }
    }
}
