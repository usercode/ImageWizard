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

namespace ImageWizard.Core.ImageLoaders.File
{
    /// <summary>
    /// FileLoader
    /// </summary>
    public class FileLoader : IImageLoader
    {
        public FileLoader(IOptions<FileLoaderSettings> settings, IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;

            FileProvider = new PhysicalFileProvider(Path.Combine(HostingEnvironment.ContentRootPath, settings.Value.Folder));
        }

        /// <summary>
        /// FileProvider
        /// </summary>
        private PhysicalFileProvider FileProvider { get; }

        /// <summary>
        /// HostingEnvironment
        /// </summary>
        private IHostingEnvironment HostingEnvironment { get; } 

        /// <summary>
        /// DeliveryType
        /// </summary>
        public string DeliveryType => "upload";

        public async Task<OriginalImage> GetAsync(string requestUri)
        {
            IFileInfo fileInfo = FileProvider.GetFileInfo(requestUri);

            if(fileInfo.Exists == false)
            {
                throw new Exception("file not found: " + requestUri);
            }

            MemoryStream mem = new MemoryStream((int)fileInfo.Length);

            using (Stream stream = fileInfo.CreateReadStream())
            {
                await stream.CopyToAsync(mem);
            }

            string extension = Path.GetExtension(fileInfo.Name).ToLower();
            string mimeType;

            switch (extension)
            {
                case ".jpg":
                    mimeType = MimeTypes.Jpeg;
                    break;

                case ".png":
                    mimeType = MimeTypes.Png;
                    break;

                case ".gif":
                    mimeType = MimeTypes.Gif;
                    break;

                case ".bmp":
                    mimeType = MimeTypes.Bitmap;
                    break;

                default:
                    throw new Exception("unknown file extension");
            }

            return new OriginalImage(requestUri, mimeType, mem.ToArray());
        }
    }
}
