using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Loaders
{
    /// <summary>
    /// FileLoader
    /// </summary>
    public class FileLoader : DataLoader<FileLoaderOptions>
    {
        public FileLoader(IOptions<FileLoaderOptions> options, IWebHostEnvironment hostingEnvironment)
            : base(options)
        {
            HostingEnvironment = hostingEnvironment;

            FileProvider = new PhysicalFileProvider(Path.Join(HostingEnvironment.ContentRootPath, options.Value.Folder));
        }

        /// <summary>
        /// FileProvider
        /// </summary>
        private PhysicalFileProvider FileProvider { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        private IWebHostEnvironment HostingEnvironment { get; }

        public override async Task<OriginalData?> GetAsync(string source, ICachedData? existingCachedImage)
        {
            IFileInfo fileInfo = FileProvider.GetFileInfo(source);

            if (fileInfo.Exists == false)
            {
                throw new Exception("file not found: " + source);
            }

            string etag = (fileInfo.Length ^ fileInfo.LastModified.UtcTicks).ToString();

            if (existingCachedImage != null)
            {
                if (existingCachedImage.Metadata.Cache.ETag == etag)
                {
                    return null;
                }
            }

            Stream stream = fileInfo.CreateReadStream();
            
            string mimeType = MimeTypes.GetByExtension(fileInfo.Name);

            return new OriginalData(mimeType, stream, new CacheSettings() { ETag = etag });
        }
    }
}
