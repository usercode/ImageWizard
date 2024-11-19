// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageWizard.MongoDB.Models;

[BsonIgnoreExtraElements]
public class MetadataModel : IMetadata
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public DateTime Created { get; set; }
    public DateTime LastAccess { get; set; }
    public string Key { get; set; }
    public string Hash { get; set; }
    public string MimeType { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long FileLength { get; set; }
    public CacheSettings Cache { get; set; } = new CacheSettings();
    public IEnumerable<string> Filters { get; set; }
    public string LoaderSource { get; set; }
    public string LoaderType { get; set; }
}
