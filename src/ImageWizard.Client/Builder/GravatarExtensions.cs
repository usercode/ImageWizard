using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// GravatarExtensions
    /// </summary>
    public static class GravatarExtensions
    {
        public static IImageFilter Gravatar(this ILoader imageUrlBuilder, string email)
        {
            string GetHashString(byte[] hash)
            {
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }

            byte[] hashBuffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

            return (IImageFilter)imageUrlBuilder.LoadData("gravatar", GetHashString(hashBuffer));
        }
    }
}
