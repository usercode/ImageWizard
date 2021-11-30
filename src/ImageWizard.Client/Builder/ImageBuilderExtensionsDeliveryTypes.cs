using ImageWizard.Client.Builder.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// ImageBuilderExtensionsDeliveryTypes
    /// </summary>
    public static class ImageBuilderExtensionsDeliveryTypes
    {
        /// <summary>
        /// Fetch file from absolute or relative url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IImageFilters Fetch(this IImageLoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.Image("fetch", url);
        }

        public static IImageFilters Azure(this IImageLoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.Image("azure", url);
        }

        public static IImageFilters AWS(this IImageLoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.Image("aws", url);
        }

        public static IImageFilters File(this IImageLoader imageUrlBuilder, string path)
        {
            return imageUrlBuilder.Image("file", path);
        }

        public static IImageFilters Gravatar(this IImageLoader imageUrlBuilder, string email)
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

            return imageUrlBuilder.Image("gravatar", GetHashString(hashBuffer));
        }

        public static IImageFilters Youtube(this IImageLoader imageUrlBuilder, string id)
        {
            return imageUrlBuilder.Image("youtube", id);
        }
    }
}
