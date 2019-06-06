using ImageWizard.ImageFormats;
using ImageWizard.Services.Types;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.Services
{
    public class FileService
    {
        public FileService(IHostingEnvironment env )
        {
            RootFolder = new DirectoryInfo(env.WebRootPath);

            
        }

        public DirectoryInfo RootFolder { get; }

        private string[] SplitSecret(string secret)
        {
            string part1 = secret.Substring(0, 2);
            string part2 = secret.Substring(2, 2);

            return new[] { part1, part2 };
        }

        public async Task<CachedImage> GetImageAsync(string secretKey)
        {
            string[] parts = SplitSecret(secretKey);

            string baseFilePath = Path.Combine(new[] { RootFolder.FullName }.Concat(parts).Concat(new[] { secretKey }).ToArray());

            FileInfo fileInfo = new FileInfo(baseFilePath);
            FileInfo fileMetadata = new FileInfo(baseFilePath + ".json");

            if (fileInfo.Exists && fileMetadata.Exists)
            {
                CachedImage cachedImage = new CachedImage();
                cachedImage.Metadata = JsonConvert.DeserializeObject<ImageMetadata>(File.ReadAllText(fileMetadata.FullName, Encoding.UTF8));
                cachedImage.Data = await File.ReadAllBytesAsync(fileInfo.FullName);

                return cachedImage;
            }
            else
            {
                return null;
            }
        }

        public async Task<CachedImage> SaveImageAsync(string secretKey, OriginalImage originalImage, IImageFormat imageFormat, byte[] transformedImageData)
        {
            string[] parts = SplitSecret(secretKey);

            //store transformed image
            DirectoryInfo sub = Directory.CreateDirectory(Path.Combine(new[] { RootFolder.FullName }.Concat(parts).ToArray()));

            FileInfo file = new FileInfo(Path.Combine(sub.FullName, secretKey));

            await File.WriteAllBytesAsync(file.FullName, transformedImageData);

            //create metadata file
            ImageMetadata imageMetadata = new ImageMetadata();
            imageMetadata.MimeType = imageFormat.MimeType;
            imageMetadata.Url = originalImage.Url;
            imageMetadata.SecretKey = secretKey;

            string metadataJson = JsonConvert.SerializeObject(imageMetadata);

            FileInfo metadatafile = new FileInfo(Path.Combine(sub.FullName, secretKey + ".json"));

            await File.WriteAllTextAsync(metadatafile.FullName, metadataJson, Encoding.UTF8);

            return new CachedImage() { Data = transformedImageData, Metadata = imageMetadata };
        }
    }
}
