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
            CacheSettings = new CacheSettings();
        }

        public ObjectId Id { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string Signature { get; set; }
        public string MimeType { get; set; }
        public string ImageSource { get; set; }
        public double? DPR { get; set; }

        public CacheSettings CacheSettings { get; set; }

    }
}
