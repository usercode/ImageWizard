using ImageWizard.AspNetCore.Builder;
using ImageWizard.AspNetCore.Builder.Types;
using ImageWizard.SharedContract.FilterTypes;
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
        public static IImageFilters Fetch(this IImageLoaderType imageUrlBuilder, string url)
        {
            imageUrlBuilder.Image("fetch", url);

            return (IImageFilters)imageUrlBuilder;
        }

        public static IImageFilters File(this IImageLoaderType imageUrlBuilder, string path)
        {
            imageUrlBuilder.Image("file", path);

            return (IImageFilters)imageUrlBuilder;
        }

        public static IImageFilters Gravatar(this IImageLoaderType imageUrlBuilder, string email)
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

            var md5 = MD5.Create();
            byte[] hashBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

            imageUrlBuilder.Image("gravatar", GetHashString(hashBuffer));

            return (IImageFilters)imageUrlBuilder;
        }

        public static IImageFilters Youtube(this IImageLoaderType imageUrlBuilder, string id)
        {
            imageUrlBuilder.Image("youtube", id);

            return (IImageFilters)imageUrlBuilder;
        }
    }
}
