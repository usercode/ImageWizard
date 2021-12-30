using ImageWizard.Core.Types;
using ImageWizard.ImageLoaders;
using ImageWizard.Services.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Core.ImageLoaders.Files
{
    /// <summary>
    /// FileLoader
    /// </summary>
    public class FileLoader : DataLoaderBase
    {
        public FileLoader(IOptions<FileLoaderOptions> options, IWebHostEnvironment hostingEnvironment)
        {
            Options = options.Value;
            HostingEnvironment = hostingEnvironment;

            FileProvider = new PhysicalFileProvider(Path.Combine(HostingEnvironment.ContentRootPath, options.Value.Folder));
        }

        /// <summary>
        /// Options
        /// </summary>
        private FileLoaderOptions Options { get; }

        /// <summary>
        /// FileProvider
        /// </summary>
        private PhysicalFileProvider FileProvider { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        private IWebHostEnvironment HostingEnvironment { get; }

        public override DataLoaderRefreshMode RefreshMode => Options.RefreshMode;

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

            MemoryStream mem = new MemoryStream((int)fileInfo.Length);

            using (Stream stream = fileInfo.CreateReadStream())
            {
                await stream.CopyToAsync(mem);
            }

            string mimeType = MimeTypes.GetByExtension(fileInfo.Name);

            mem.Seek(0, SeekOrigin.Begin);

            return new OriginalData(mimeType, mem.ToArray(), new CacheSettings() { ETag = etag });
        }
    }
}
