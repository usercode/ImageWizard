// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace ImageWizard.Loaders;

/// <summary>
/// FileLoader
/// </summary>
public class FileLoader : Loader<FileLoaderOptions>
{
    public FileLoader(IOptions<FileLoaderOptions> options, IWebHostEnvironment hostingEnvironment)
        : base(options)
    {
        HostingEnvironment = hostingEnvironment;

        if (Path.IsPathRooted(options.Value.Folder))
        {
            FileProvider = new PhysicalFileProvider(options.Value.Folder);            
        }
        else
        {
            FileProvider = new PhysicalFileProvider(Path.Join(HostingEnvironment.ContentRootPath, options.Value.Folder));
        }
    }

    /// <summary>
    /// FileProvider
    /// </summary>
    private IFileProvider FileProvider { get; }

    /// <summary>
    /// HostingEnvironment
    /// </summary>
    private IWebHostEnvironment HostingEnvironment { get; }

    public override async Task<LoaderResult> GetAsync(string source, CachedData? existingCachedImage)
    {
        IFileInfo fileInfo = FileProvider.GetFileInfo(source);

        if (fileInfo.Exists == false)
        {
            return LoaderResult.NotFound();
        }

        if (fileInfo.Length > Options.Value.MaxLoaderSourceLength)
        {
            return LoaderResult.Failed();
        }

        string etag = fileInfo.GetEtag();

        if (existingCachedImage != null)
        {
            if (existingCachedImage.Metadata.Cache.ETag == etag)
            {
                return LoaderResult.NotModified();
            }
        }

        Stream stream = fileInfo.CreateReadStream();
        
        string mimeType = MimeTypes.GetByExtension(fileInfo.Name);

        return LoaderResult.Success(new OriginalData(mimeType, stream, new CacheSettings() { ETag = etag }.ApplyLoaderOptions(Options.Value)));
    }
}
