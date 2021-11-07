using ImageWizard.Client.Builder.Types;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ImageWizard
{
    public static class ImageBuilderExtensionsDeliveryTypesLocal
    {
        /// <summary>
        /// Fetch file from wwwroot folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="addFingerprint"></param>
        /// <returns></returns>
        public static IImageFilters FetchLocalFile(this IImageLoaderType imageBuilder, string path, bool addFileVersion = true)
        {
            if (addFileVersion == true)
            {
                IFileInfo file = imageBuilder.HostEnvironment.WebRootFileProvider.GetFileInfo(path);

                string hash = $"{file.Length}#{file.LastModified.Ticks}";

                using var sha256 = SHA256.Create();

                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(hash));
                string hashBase64 = WebEncoders.Base64UrlEncode(hashBytes);

                path += $"?v={hashBase64}";
            }

            imageBuilder.Image("fetch", path);

            return (IImageFilters)imageBuilder;
        }
    }
}
