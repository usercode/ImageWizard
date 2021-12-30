using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.Utils
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream input)
        {
            MemoryStream mem = new MemoryStream();

            input.CopyTo(mem);

            if (input.CanSeek)
            {
                input.Seek(0, SeekOrigin.Begin);
            }

            return mem.ToArray();

            //byte[] buffer = new byte[input.Length];

            //int readBytes;
            //int offset = 0;

            //while ((readBytes = input.Read(buffer, offset, 4096)) > 0)
            //{
            //    offset += readBytes;
            //}

            //return buffer;
        }
    }
}