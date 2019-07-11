using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.Types;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.ImageStorages
{
    /// <summary>
    /// FileCache
    /// </summary>
    public class FileCache : IImageCache
    {
        public FileCache(IOptions<FileCacheSettings> settings, IHostingEnvironment hostingEnvironment)
        {
            Settings = settings;
            HostingEnvironment = hostingEnvironment;

            Serializer = new CachedFileSerializer();
        }

        /// <summary>
        /// Settings
        /// </summary>
        public IOptions<FileCacheSettings> Settings { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        private IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Serializer
        /// </summary>
        private CachedFileSerializer Serializer { get; }

        private string[] SplitSecret(string secret)
        {
            string part1 = secret.Substring(0, 1);
            string part2 = secret.Substring(1, 1);
            string part3 = secret.Substring(2, 1);
            string part4 = secret.Substring(3, 1);

            return new[] { part1, part2, part3, part4 };
        }

        private string ToHex(string key)
        {
            //convert signature to hex for the filestore
            byte[] buf = Encoding.UTF8.GetBytes(key);
            string signatureHex = buf.Aggregate(string.Empty, (a, b) => a += b.ToString("x2"));

            return signatureHex;
        }

        public async Task<CachedImage> ReadAsync(string key)
        {
            string signatureHex = ToHex(key);

            string[] parts = SplitSecret(signatureHex);

            string baseFilePath = Path.Combine(new[] { HostingEnvironment.ContentRootPath }.Concat(new[] { Settings.Value.Folder  }).Concat(parts).Concat(new[] { signatureHex }).ToArray());

            FileInfo fileInfo = new FileInfo(baseFilePath);

            if (fileInfo.Exists == false)
            {
                return null;
            }

            //read cache image from disk
            MemoryStream memImage = new MemoryStream((int)fileInfo.Length);

            using (Stream fs = fileInfo.OpenRead())
            {
                await fs.CopyToAsync(memImage);
            }

            memImage.Seek(0, SeekOrigin.Begin);

            CachedImage cachedImage = Serializer.Read(memImage);

            return cachedImage;
        }

        public async Task WriteAsync(string key, CachedImage cachedImage)
        {
            byte[] buffer = Serializer.Write(cachedImage);

            string signatureHex = ToHex(key);

            string[] parts = SplitSecret(signatureHex);

            //store transformed image
            DirectoryInfo sub = Directory.CreateDirectory(Path.Combine(new[] { HostingEnvironment.ContentRootPath }.Concat(new[] { Settings.Value.Folder }).Concat(parts).ToArray()));

            //write to file
            FileInfo file = new FileInfo(Path.Combine(sub.FullName, signatureHex));

            using (Stream fs = file.OpenWrite())
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
