﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core
{
    /// <summary>
    /// Extenions
    /// </summary>
    public static class Extensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            MemoryStream mem = new MemoryStream();

            await stream.CopyToAsync(mem);

            return mem.ToArray();
        }

        public static string GetTagUnquoted(this EntityTagHeaderValue value)
        {
            return value.Tag.GetTagUnquoted();
        }

        public static string GetTagUnquoted(this string value)
        {
            return value.Substring(1, value.Length - 2);
        }
    }
}
