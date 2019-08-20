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
            Cache = new CacheSettings();
        }

        public ObjectId Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Signature { get; set; }
        public string Hash { get; set; }
        public string MimeType { get; set; }
        public double? DPR { get; set; }
        public bool NoImageCache { get; set; }
        public int FileLength { get; set; }
        public CacheSettings Cache { get; set; }
        public string[] Filters { get; set; }
        public string LoaderSource { get; set; }
        public string LoaderType { get; set; }
    }
}
