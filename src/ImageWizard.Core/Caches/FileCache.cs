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
                Folder = new DirectoryInfo(Path.Join(hostingEnvironment.ContentRootPath, settings.Value.Folder));
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

            string basePath = Path.Join(parts);
            
            FileInfo metadataFile = new FileInfo(basePath + ".meta");
            FileInfo blobFile = new FileInfo(basePath + ".blob");

            if (metadataFile.Exists == false || blobFile.Exists == false)
            {
                return null;
            }

            using Stream metadataStream = metadataFile.OpenRead();

            Metadata? metadata = JsonSerializer.Deserialize<Metadata>(metadataStream);

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            return new CachedData(metadata, () => Task.FromResult<Stream>(blobFile.OpenRead()));
        }

        public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
        {
            //create json
            byte[] metadataJson = JsonSerializer.SerializeToUtf8Bytes(metadata, new JsonSerializerOptions() { WriteIndented = true });

            string[] parts = KeyToPath(key);

            //create folder structure
            DirectoryInfo sub = Directory.CreateDirectory(Path.Join(parts[0..^1]));

            FileInfo metadataFile = new FileInfo(Path.Join(sub.FullName, $"{parts[^1]}.meta"));
            FileInfo blobFile = new FileInfo(Path.Join(sub.FullName, $"{parts[^1]}.blob"));

            //delete old files if exist
            metadataFile.Delete();
            blobFile.Delete();
            
            using Stream jsonStream = new MemoryStream(metadataJson);
            using Stream metadataStream = metadataFile.OpenWrite();

            await jsonStream.CopyToAsync(metadataStream);

            //write data
            using Stream blobStream = blobFile.OpenWrite();

            await stream.CopyToAsync(blobStream);
        }
    }
}
