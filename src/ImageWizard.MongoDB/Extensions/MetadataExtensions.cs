// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.MongoDB.Models;

namespace ImageWizard.MongoDB;

internal static class MetadataExtensions
{
    public static MetadataModel ToModel(this Metadata metadata)
    {
        return new MetadataModel()
        {
            Created = metadata.Created,
            LastAccess = metadata.LastAccess,
            Key = metadata.Key,
            Hash = metadata.Hash,
            MimeType = metadata.MimeType,
            Width = metadata.Width,
            Height = metadata.Height,
            FileLength = metadata.FileLength,
            Cache = metadata.Cache,
            Filters = metadata.Filters,
            LoaderType = metadata.LoaderType,
            LoaderSource = metadata.LoaderSource
        };
    }

    public static Metadata ToMetadata(this MetadataModel metadata)
    {
        return new Metadata()
        {
            Created = metadata.Created,
            LastAccess = metadata.LastAccess,
            Key = metadata.Key,
            Hash = metadata.Hash,
            MimeType = metadata.MimeType,
            Width = metadata.Width,
            Height = metadata.Height,
            FileLength = metadata.FileLength,
            Cache = metadata.Cache,
            Filters = metadata.Filters,
            LoaderType = metadata.LoaderType,
            LoaderSource = metadata.LoaderSource
        };
    }
}
