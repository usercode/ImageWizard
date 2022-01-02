﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard.MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class ImageMetadataModel : IMetadata
    {
        public ImageMetadataModel()
        {
            Id = ObjectId.GenerateNewId();
            Cache = new CacheSettings();
        }

        public ObjectId Id { get; set; }
        public DateTime? Created { get; set; }
        public string Key { get; set; }
        public string Hash { get; set; }
        public string MimeType { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? DPR { get; set; }
        public long? FileLength { get; set; }
        public CacheSettings Cache { get; set; }
        public IEnumerable<string> Filters { get; set; }
        public string LoaderSource { get; set; }
        public string LoaderType { get; set; }
    }
}
