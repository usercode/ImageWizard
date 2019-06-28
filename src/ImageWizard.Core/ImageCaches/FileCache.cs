using ImageWizard.Core.ImageCaches;
using ImageWizard.ImageFormats;
using ImageWizard.ImageFormats.Base;
using ImageWizard.Services.Types;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard.ImageStorages
{
    public class FileCache : IImageCache
    {
        public FileCache(FileCacheSettings settings)
        {
            Settings = settings;
        }

        public FileCacheSettings Settings { get; }

        private string[] SplitSecret(string secret)
        {
            string part1 = secret.Substring(0, 1);
            string part2 = secret.Substring(1, 1);
            string part3 = secret.Substring(2, 1);
            string part4 = secret.Substring(3, 1);

            return new[] { part1, part2, part3, part4 };
        }

        private string ToHex(string signature)
        {
            //convert signature to hex for the filestore
            byte[] buf = Encoding.UTF8.GetBytes(signature);
            string signatureHex = buf.Aggregate(string.Empty, (a, b) => a += b.ToString("x2"));

            return signatureHex;
        }

        public async Task<CachedImage> GetAsync(string signature)
        {
            string signatureHex = ToHex(signature);

            string[] parts = SplitSecret(signatureHex);

            string baseFilePath = Path.Combine(new[] { Settings.RootFolder.FullName }.Concat(parts).Concat(new[] { signatureHex }).ToArray());

            FileInfo fileInfo = new FileInfo(baseFilePath);

            if (fileInfo.Exists == false)
            {
                return null;
            }

            byte[] fileBuffer = File.ReadAllBytes(fileInfo.FullName);
            BinaryReader reader = new BinaryReader(new MemoryStream(fileBuffer));

            //read version
            int version = reader.ReadInt32();

            //read metadata
            int len = reader.ReadInt32();
            byte[] metadataBuffer = reader.ReadBytes(len);

            string metadataString = Encoding.UTF8.GetString(metadataBuffer);

            //read transformed image
            len = reader.ReadInt32();
            byte[] transformedImageBUffer = reader.ReadBytes(len);

            //cached image
            CachedImage cachedImage = new CachedImage();
            cachedImage.Metadata = JsonConvert.DeserializeObject<ImageMetadata>(metadataString);
            cachedImage.Data = transformedImageBUffer;

            return cachedImage;
        }

        public async Task<CachedImage> SaveAsync(string signature, OriginalImage originalImage, IImageFormat imageFormat, byte[] transformedImageData)
        {
            string signatureHex = ToHex(signature);

            string[] parts = SplitSecret(signatureHex);

            MemoryStream cachedFileData = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(cachedFileData);
            
            //write file version
            writer.Write(1);

            //create metadata
            ImageMetadata imageMetadata = new ImageMetadata();
            imageMetadata.MimeType = imageFormat.MimeType;
            imageMetadata.Url = originalImage.Url;
            imageMetadata.Signature = signatureHex;

            string metadataJson = JsonConvert.SerializeObject(imageMetadata);
            byte[] metadataBuffer = Encoding.UTF8.GetBytes(metadataJson);

            writer.Write(metadataBuffer.Length);
            writer.Write(metadataBuffer);

            //store transformed image
            DirectoryInfo sub = Directory.CreateDirectory(Path.Combine(new[] { Settings.RootFolder.FullName }.Concat(parts).ToArray()));

            writer.Write(transformedImageData.Length);
            writer.Write(transformedImageData);

            //write to file
            FileInfo file = new FileInfo(Path.Combine(sub.FullName, signatureHex));
            
            File.WriteAllBytes(file.FullName, cachedFileData.ToArray());

            return new CachedImage() { Data = transformedImageData, Metadata = imageMetadata };
        }
    }
}
