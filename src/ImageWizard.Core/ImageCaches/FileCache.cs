using ImageWizard.Core;
using ImageWizard.Core.ImageCaches;
using ImageWizard.Core.ImageLoaders.Files;
using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.ImageStorages
{
    /// <summary>
    /// FileCache
    /// </summary>
    public class FileCache : IImageCache
    {
        public FileCache(IOptions<FileCacheSettings> settings, IWebHostEnvironment hostingEnvironment)
        {
            Settings = settings;
            HostingEnvironment = hostingEnvironment;

            FileProvider = new PhysicalFileProvider(Path.Combine(hostingEnvironment.ContentRootPath, settings.Value.Folder));
        }

        /// <summary>
        /// Settings
        /// </summary>
        public IOptions<FileCacheSettings> Settings { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        private IWebHostEnvironment HostingEnvironment { get; }

        /// <summary>
        /// FileProvider
        /// </summary>
        private IFileProvider FileProvider { get; }

        private string[] SplitKey(string secret)
        {
            string part1 = secret.Substring(0, 2);
            string part2 = secret.Substring(2, 2);
            string part3 = secret.Substring(4, 2);
            string part4 = secret.Substring(6, 2);
            string part_last = secret.Substring(8);

            return new[] { part1, part2, part3, part4, part_last };
        }

        public async Task<ICachedImage> ReadAsync(string key)
        {
            string[] parts = SplitKey(key);

            string basePath = Path.Combine(parts);

            IFileInfo fileInfoData = FileProvider.GetFileInfo(basePath);
            IFileInfo fileInfoMetadata = FileProvider.GetFileInfo(basePath + ".meta");

            if (fileInfoMetadata.Exists == false || fileInfoData.Exists == false)
            {
                return null;
            }

            MemoryStream mem = new MemoryStream((int)fileInfoMetadata.Length);

            using (Stream fs = fileInfoMetadata.CreateReadStream())
            {
                await fs.CopyToAsync(mem);
            }

            string json = Encoding.UTF8.GetString(mem.ToArray());

            ImageMetadata metadata = JsonSerializer.Deserialize<ImageMetadata>(json);

            return new CachedImage(metadata, () => Task.FromResult(fileInfoData.CreateReadStream()));
        }

        public async Task WriteAsync(string key, ICachedImage cachedImage)
        {
            string[] parts = SplitKey(key);

            //store transformed image
            DirectoryInfo sub = Directory.CreateDirectory(Path.Combine(new[] { HostingEnvironment.ContentRootPath }.Concat(new[] { Settings.Value.Folder }).Concat(parts.Take(parts.Length-1)).ToArray()));

            //write to file
            FileInfo fileInfoMetadata = new FileInfo(Path.Combine(sub.FullName, parts.Last() + ".meta"));
            
            //write metadata
            string json = JsonSerializer.Serialize(cachedImage.Metadata, new JsonSerializerOptions() { WriteIndented = true } );
            byte[] metadataBuffer = Encoding.UTF8.GetBytes(json);

            using (Stream fs = fileInfoMetadata.OpenWrite())
            {
                await fs.WriteAsync(metadataBuffer, 0, metadataBuffer.Length);
            }

            //write data
            FileInfo fileInfoData = new FileInfo(Path.Combine(sub.FullName, parts.Last()));

            using (Stream fs = fileInfoData.OpenWrite())
            using (Stream cachedImageStream = await cachedImage.OpenReadAsync())
            {
                await cachedImageStream.CopyToAsync(fs);
            }
        }
    }
}
