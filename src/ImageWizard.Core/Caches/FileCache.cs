using ImageWizard.Core;
using ImageWizard.Core.Types;
using ImageWizard.Metadatas;
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

namespace ImageWizard.Caches
{
    /// <summary>
    /// FileCache
    /// </summary>
    public class FileCache : ICache
    {
        public FileCache(IOptions<FileCacheSettings> settings, IWebHostEnvironment hostingEnvironment)
        {
            Settings = settings;

            if (Path.IsPathFullyQualified(settings.Value.Folder))
            {
                Folder = new DirectoryInfo(settings.Value.Folder);
            }
            else
            {
                Folder = new DirectoryInfo(Path.Combine(hostingEnvironment.ContentRootPath, settings.Value.Folder));
            }            
        }

        /// <summary>
        /// Settings
        /// </summary>
        public IOptions<FileCacheSettings> Settings { get; }

        /// <summary>
        /// FileProvider
        /// </summary>
        private DirectoryInfo Folder { get; }

        private string[] KeyToPath(string secret)
        {
            string part1 = secret.Substring(0, 2);
            string part2 = secret.Substring(2, 2);
            string part3 = secret.Substring(4, 2);
            string part4 = secret.Substring(6, 2);
            string part_last = secret.Substring(8);

            return new[] { Folder.FullName, part1, part2, part3, part4, part_last };
        }

        public async Task<ICachedData?> ReadAsync(string key)
        {
            string[] parts = KeyToPath(key);

            string basePath = Path.Combine(parts);

            FileInfo fileInfoData = new FileInfo(basePath);
            FileInfo fileInfoMetadata = new FileInfo(basePath + ".meta");

            if (fileInfoMetadata.Exists == false || fileInfoData.Exists == false)
            {
                return null;
            }

            MemoryStream mem = new MemoryStream((int)fileInfoMetadata.Length);

            using (Stream fs = fileInfoMetadata.OpenRead())
            {
                await fs.CopyToAsync(mem);
            }

            string json = Encoding.UTF8.GetString(mem.ToArray());

            Metadata? metadata = JsonSerializer.Deserialize<Metadata>(json);

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            return new CachedData(metadata, () => Task.FromResult<Stream>(fileInfoData.OpenRead()));
        }

        public async Task WriteAsync(string key, ICachedData cachedData)
        {
            string[] parts = KeyToPath(key);

            //create folder structure
            DirectoryInfo sub = Directory.CreateDirectory(Path.Combine(parts.Take(parts.Length - 1).ToArray()));

            //write to file
            FileInfo fileInfoMetadata = new FileInfo(Path.Combine(sub.FullName, parts.Last() + ".meta"));

            //write metadata
            string json = JsonSerializer.Serialize(cachedData.Metadata, new JsonSerializerOptions() { WriteIndented = true });
            Stream metadataStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            using (Stream fs = fileInfoMetadata.OpenWrite())
            {
                await metadataStream.CopyToAsync(fs);
            }

            //write data
            FileInfo fileInfoData = new FileInfo(Path.Combine(sub.FullName, parts.Last()));

            using (Stream fs = fileInfoData.OpenWrite())
            using (Stream cachedImageStream = await cachedData.OpenReadAsync())
            {
                await cachedImageStream.CopyToAsync(fs);
            }
        }
    }
}
