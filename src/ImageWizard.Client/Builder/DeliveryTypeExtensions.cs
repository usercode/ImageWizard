using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard.Client
{
    /// <summary>
    /// DeliveryTypeExtensions
    /// </summary>
    public static class DeliveryTypeExtensions
    {
        /// <summary>
        /// Fetch file from absolute or relative url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IFilter Fetch(this ILoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.LoadData("fetch", url);
        }

        /// <summary>
        /// Fetch file from wwwroot folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="addFingerprint"></param>
        /// <returns></returns>
        public static IFilter FetchLocalFile(this ILoader imageBuilder, string path, bool addFileVersion = true)
        {
            if (addFileVersion == true)
            {
                IWebHostEnvironment env = imageBuilder.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                IFileInfo file = env.WebRootFileProvider.GetFileInfo(path);

                string hash = $"{file.Length}#{file.LastModified.Ticks}";

                byte[] hashBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(hash));
                string hashBase64 = WebEncoders.Base64UrlEncode(hashBytes);

                path += $"?v={hashBase64}";
            }

            return imageBuilder.LoadData("fetch", path);
        }

        public static IFilter Azure(this ILoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.LoadData("azure", url);
        }

        public static IFilter AWS(this ILoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.LoadData("aws", url);
        }

        public static IFilter File(this ILoader imageUrlBuilder, string path)
        {
            return imageUrlBuilder.LoadData("file", path);
        }
    }
}
