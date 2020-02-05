using ImageWizard.Core.Types;
using ImageWizard.Filters.ImageFormats;
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
    public class FileLoader : ImageLoaderBase
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

        public override ImageLoaderRefreshMode RefreshMode => Options.RefreshMode;

        public override async Task<OriginalImage> GetAsync(string source, ICachedImage existingCachedImage)
        {
            IFileInfo fileInfo = FileProvider.GetFileInfo(source);

            if (fileInfo.Exists == false)
            {
                throw new Exception("file not found: " + source);
            }

            string etag = (fileInfo.Length ^ fileInfo.LastModified.UtcTicks).ToString();

            if(existingCachedImage != null)
            {
                if(existingCachedImage.Metadata.Cache.ETag == etag)
                {
                    return null;
                }
            }

            MemoryStream mem = new MemoryStream((int)fileInfo.Length);

            using (Stream stream = fileInfo.CreateReadStream())
            {
                await stream.CopyToAsync(mem);
            }

            string mimeType = ImageFormatHelper.GetMimeTypeByExtension(fileInfo.Name);

            return new OriginalImage(mimeType, mem.ToArray(), new CacheSettings() { ETag = etag });
        }
    }
}
