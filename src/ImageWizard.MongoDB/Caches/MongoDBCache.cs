﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using ImageWizard.MongoDB.Models;
using ImageWizard.Caches;
using ImageWizard.Cleanup;

namespace ImageWizard.MongoDB;

/// <summary>
/// MongoDBCache
/// </summary>
public class MongoDBCache : ICache, ICleanupCache, ILastAccessCache
{
    public MongoDBCache(IOptions<MongoDBCacheOptions> settings, ICacheLock cacheLock)
    {
        var mongoSetttings = new MongoClientSettings() { Server = new MongoServerAddress(settings.Value.Hostname) };

        if (string.IsNullOrEmpty(settings.Value.Username) == false)
        {
            mongoSetttings.Credential = MongoCredential.CreateCredential("admin", settings.Value.Username, settings.Value.Password);
        }

        CacheLock = cacheLock;

        Client = new MongoClient(mongoSetttings);

        Database = Client.GetDatabase(settings.Value.Database);

        Metadata = Database.GetCollection<MetadataModel>("Metadata");
        Blob = new GridFSBucket(Database, new GridFSBucketOptions() { BucketName = "Data" });

        //create index
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.Key)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.Hash)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.Created)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.LastAccess)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.MimeType)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.LoaderType)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.LoaderSource)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.FileLength)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.Width)));
        Metadata.Indexes.CreateOne(new CreateIndexModel<MetadataModel>(Builders<MetadataModel>.IndexKeys.Ascending(x => x.Height)));

    }

    /// <summary>
    /// Client
    /// </summary>
    public MongoClient Client { get; }

    /// <summary>
    /// Database
    /// </summary>
    public IMongoDatabase Database { get; }

    /// <summary>
    /// Metadata
    /// </summary>
    public IMongoCollection<MetadataModel> Metadata { get; set; }

    /// <summary>
    /// Blob
    /// </summary>
    public IGridFSBucket Blob { get; }

    /// <summary>
    /// CacheLock
    /// </summary>
    private ICacheLock CacheLock { get; }

    public async Task CleanupAsync(CleanupReason reason, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await Metadata.AsQueryable()
                                                    .Where(reason.GetExpression<MetadataModel>())
                                                    .Take(100)
                                                    .ToListAsync();

            if (items.Count == 0)
            {
                break;
            }

            foreach (MetadataModel item in items)
            {
                //set writer lock
                using var w = CacheLock.WriterLockAsync(item.Key);

                //delete meta
                await Metadata.DeleteOneAsync(Builders<MetadataModel>.Filter.Eq(x => x.Key, item.Key));

                //delete blob
                var blobCursor = await Blob.FindAsync(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, item.Key));

                GridFSFileInfo blob = await blobCursor.FirstAsync();

                await Blob.DeleteAsync(blob.Id);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(80));
        }        
    }

    public async Task<CachedData?> ReadAsync(string key)
    {
        MetadataModel foundMetadata = await Metadata
                                                    .AsQueryable()
                                                    .FirstOrDefaultAsync(x => x.Key == key);

        if (foundMetadata == null)
        {
            return null;
        }

        CachedData cachedImage = new CachedData(foundMetadata.ToMetadata(), async () => await Blob.OpenDownloadStreamByNameAsync(key));

        return cachedImage;
    }

    public async Task SetLastAccessAsync(string key, DateTime dateTime)
    {
        await Metadata.UpdateOneAsync(
                                Builders<MetadataModel>.Filter.Eq(x => x.Key, key),
                                Builders<MetadataModel>.Update.Set(x => x.LastAccess, dateTime));
    }

    public async Task WriteAsync(Metadata metadata, Stream stream)
    {
        //upload metadata
        await Metadata.ReplaceOneAsync(Builders<MetadataModel>.Filter.Eq(x => x.Key, metadata.Key), metadata.ToModel(), new ReplaceOptions() { IsUpsert = true });

        //upload cached data
        await Blob.UploadFromStreamAsync(metadata.Key, stream);
    }
}
