using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.Core
{
    /// <summary>
    /// Extenions
    /// </summary>
    public static class Extensions
    {
        public static string ToHexcode(this string value)
        {
            return ToHexcode(Encoding.UTF8.GetBytes(value));
        }

        public static string ToHexcode(this byte[] buffer)
        {
            StringBuilder stringBuilder = new StringBuilder(buffer.Length * 2);
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
