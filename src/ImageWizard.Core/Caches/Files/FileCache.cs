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
    /// FileCache
    /// </summary>
    public class FileCache : ICache
    {
        private static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

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

        private FileCacheLocations GetFileCacheLocations(string secret)
        {
            string folders = Path.Join(
                                    secret.AsSpan(0, 2),
                                    secret.AsSpan(2, 2),
                                    secret.AsSpan(4, 2),
                                    secret.AsSpan(6, 2));

            ReadOnlySpan<char> filename = secret.AsSpan(8);

            string basePath = Path.Join(Folder.FullName, folders, filename);

            return new FileCacheLocations(new FileInfo(basePath + ".meta"), new FileInfo(basePath + ".blob"));
        }

        public async Task<ICachedData?> ReadAsync(string key)
        {
            FileCacheLocations locations = GetFileCacheLocations(key);

            if (locations.MetaFile.Exists == false || locations.BlobFile.Exists == false)
            {
                return null;
            }

            using Stream metadataStream = locations.MetaFile.OpenRead();

            Metadata? metadata = await JsonSerializer.DeserializeAsync<Metadata>(metadataStream);

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            return new CachedData(metadata, () => Task.FromResult<Stream>(locations.BlobFile.OpenRead()));
        }

        public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
        {
            FileCacheLocations locations = GetFileCacheLocations(key);

            if (locations.MetaFile.Directory != null)
            {
                //create folder structure
                locations.MetaFile.Directory.Create();
            }

            using Stream metadataStream = locations.MetaFile.OpenWrite();

            //delete existing data
            metadataStream.SetLength(0);

            await JsonSerializer.SerializeAsync(metadataStream, metadata, JsonSerializerOptions);

            //write data
            using Stream blobStream = locations.BlobFile.OpenWrite();

            //delete existing data
            blobStream.SetLength(0);

            await stream.CopyToAsync(blobStream);
        }
    }
}
