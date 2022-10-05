// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.FileProviders;

namespace ImageWizard;

public static class FileInfoExtensions
{
    public static string GetEtag(this IFileInfo fileInfo)
    {
        string etag = (fileInfo.Length ^ fileInfo.LastModified.UtcTicks).ToString();

        return etag;
    }

    public static string GetEtag(this FileInfo fileInfo)
    {
        string etag = (fileInfo.Length ^ fileInfo.LastWriteTimeUtc.Ticks).ToString();

        return etag;
    }

    public static CachedData ToCachedData(this FileInfo fileInfo)
    {
        return new CachedData(
                                new Metadata()
                                {
                                    Created = fileInfo.CreationTimeUtc,
                                    LastAccess = DateTime.UtcNow,
                                    MimeType = MimeTypes.GetByExtension(fileInfo.Name),
                                    FileLength = fileInfo.Length,
                                    Hash = fileInfo.GetEtag()
                                },
                                () => Task.FromResult<Stream>(fileInfo.OpenRead()));
    }
}
