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
        /// Fetch file from absolute or relative url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IFilter Fetch(this ILoader imageUrlBuilder, string url)
        {
            return imageUrlBuilder.LoadData("fetch", url);
        }

        /// <summary>
        /// Fetch file from wwwroot folder with fingerprint.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IFilter FetchLocalFile(this ILoader imageBuilder, string path, int? hashNameLength = null)
        {
            if (hashNameLength != null)
            {
                if (hashNameLength < 1 || hashNameLength > 43)
                {
                    throw new ArgumentOutOfRangeException(nameof(hashNameLength));
                }
            }

            IWebHostEnvironment env = imageBuilder.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            IFileInfo file = env.WebRootFileProvider.GetFileInfo(path);

            string hash = $"{file.Length}_{file.LastModified.UtcTicks}";

            Span<byte> hashBufferSpan = stackalloc byte[32];

            SHA256.HashData(Encoding.UTF8.GetBytes(hash), hashBufferSpan);

            string hashBase64 = WebEncoders.Base64UrlEncode(hashBufferSpan);

            if (hashNameLength == null)
            {
                path += $"?v={hashBase64}";
            }
            else
            {
                path += $"?v={hashBase64.AsSpan(0, hashNameLength.Value)}";
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
