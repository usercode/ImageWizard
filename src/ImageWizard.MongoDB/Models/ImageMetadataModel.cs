using ImageWizard.Core.Types;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.MongoDB.Models
{
    public class ImageMetadataModel : IImageMetadata
    {
        public ImageMetadataModel()
        {
            Id = ObjectId.GenerateNewId();
        }

        public ObjectId Id { get; set; }

        public string Signature { get; set; }
        public string MimeType { get; set; }
        public string Url { get; set; }
    }
}
