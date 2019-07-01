using ImageWizard.Core.Types;
using ImageWizard.Services.Types;
using ImageWizard.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageWizard.Core.ImageCaches
{
    /// <summary>
    /// CachedFileSerializer
    /// </summary>
    class CachedFileSerializer
    {
        public CachedImage Read(byte[] buffer)
        {
            return Read(new MemoryStream(buffer));
        }

        public CachedImage Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            //read version
            int version = reader.ReadInt32();

            //read metadata
            int len = reader.ReadInt32();
            byte[] metadataBuffer = reader.ReadBytes(len);

            string metadataString = Encoding.UTF8.GetString(metadataBuffer);

            //read transformed image
            len = reader.ReadInt32();
            byte[] transformedImageBUffer = reader.ReadBytes(len);

            ImageMetadata metadata =  JsonConvert.DeserializeObject<ImageMetadata>(metadataString);

            return new CachedImage() { Data = transformedImageBUffer, Metadata = metadata };
        }

        public byte[] Write(CachedImage cachedImage)
        {
            MemoryStream cachedFileData = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(cachedFileData);

            //write file version
            writer.Write(1);

            string metadataJson = JsonConvert.SerializeObject(cachedImage.Metadata);
            byte[] metadataBuffer = Encoding.UTF8.GetBytes(metadataJson);

            //image metadata
            writer.Write(metadataBuffer.Length);
            writer.Write(metadataBuffer);

            //image buffer
            writer.Write(cachedImage.Data.Length);
            writer.Write(cachedImage.Data);

            cachedFileData.Seek(0, SeekOrigin.Begin);

            return cachedFileData.ToArray();
        }
    }
}
