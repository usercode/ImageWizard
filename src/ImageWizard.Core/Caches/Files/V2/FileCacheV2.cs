using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImageWizard.Caches
{
    /// <summary>
    /// FileCache with data deduplication
    /// </summary>
    public class FileCacheV2 : ICache
    {
        protected static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

        public FileCacheV2(IOptions<FileCacheSettings> settings, IWebHostEnvironment hostingEnvironment)
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
        /// Folder
        /// </summary>
        public DirectoryInfo Folder { get; }

        protected FileInfo GetMetaFile(string key)
        {
            return GetFile("meta", key);
        }

        protected FileInfo GetBlobFile(string key)
        {
            return GetFile("blob", key);
        }

        protected FileInfo GetFile(string type, string key)
        {
            string folders = Path.Join(
                                    key.AsSpan(0, 2),
                                    key.AsSpan(2, 2),
                                    key.AsSpan(4, 2),
                                    key.AsSpan(6, 2));

            ReadOnlySpan<char> filename = key.AsSpan(8);

            string basePath = Path.Join(Folder.FullName, type, folders, filename);

            return new FileInfo(basePath + $".{type}");
        }

        public async Task<ICachedData?> ReadAsync(string key)
        {
            FileInfo metafile = GetMetaFile(key);

            if (metafile.Exists == false)
            {
                return null;
            }

            using Stream metadataStream = metafile.OpenRead();

            Metadata? metadata = await JsonSerializer.DeserializeAsync<Metadata>(metadataStream);

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            FileInfo blobFile = GetBlobFile(metadata.Hash);

            if (blobFile.Exists == false)
            {
                return null;
            }

            return new CachedData(metadata, () => Task.FromResult<Stream>(blobFile.OpenRead()));
        }

        public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
        {
            FileInfo metaFile = GetMetaFile(key);

            if (metaFile.Directory != null)
            {
                //create folder structure for meta file
                metaFile.Directory.Create();
            }

            using Stream metadataStream = metaFile.OpenWrite();

            //delete existing data
            metadataStream.SetLength(0);

            await JsonSerializer.SerializeAsync(metadataStream, metadata, JsonSerializerOptions);

            FileInfo blobFile = GetBlobFile(metadata.Hash);

            if (blobFile.Directory != null)
            {
                //create folder structure for blob file
                blobFile.Directory.Create();
            }

            //write data
            using Stream blobStream = blobFile.OpenWrite();

            //delete existing data
            blobStream.SetLength(0);

            await stream.CopyToAsync(blobStream);
        }
    }
}
