// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System;
using System.Threading.Tasks;
using Mongo=MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using ImageWizard.MongoDB.Models;
using System.IO;
using ImageWizard.Caches;

namespace ImageWizard.MongoDB
{
    /// <summary>
    /// MongoDBCache
    /// </summary>
    public class MongoDBCache : ICache
    {
        public MongoDBCache(IOptions<MongoDBCacheOptions> settings)
        {
            var mongoSetttings = new Mongo.MongoClientSettings() { Server = new Mongo.MongoServerAddress(settings.Value.Hostname) };
            
            if(settings.Value.Username != null)
            {
                mongoSetttings.Credential = MongoCredential.CreateCredential(settings.Value.Database, settings.Value.Username, settings.Value.Password);
            }

            Client = new Mongo.MongoClient(mongoSetttings);

            Database = Client.GetDatabase(settings.Value.Database);

            ImageMetadatas = Database.GetCollection<ImageMetadataModel>("ImageMetadata");
            ImageBuffer = new GridFSBucket(Database, new GridFSBucketOptions() { BucketName = "ImageBuffer" });

            //create index
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.Key)));
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.Created)));
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.MimeType)));
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.DPR)));
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.Width)));
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.Height)));

        }

        /// <summary>
        /// Client
        /// </summary>
        public Mongo.MongoClient Client { get; }

        /// <summary>
        /// Database
        /// </summary>
        public Mongo.IMongoDatabase Database { get; }

        /// <summary>
        /// ImageMetadatas
        /// </summary>
        public Mongo.IMongoCollection<ImageMetadataModel> ImageMetadatas { get; set; }

        /// <summary>
        /// ImageDatas
        /// </summary>
        public IGridFSBucket ImageBuffer { get; }

        public async Task<ICachedData> ReadAsync(string key)
        {
            ImageMetadataModel foundMetadata = await ImageMetadatas
                                                            .AsQueryable()
                                                            .FirstOrDefaultAsync(x => x.Key == key);

            if(foundMetadata == null)
            {
                return null;
            }

            CachedData cachedImage = new CachedData(foundMetadata, async () => await ImageBuffer.OpenDownloadStreamByNameAsync(key));

            return cachedImage;
        }

        public async Task WriteAsync(string key, IMetadata metadata, Stream stream)
        {
            ImageMetadataModel model = new ImageMetadataModel()
            {
                Created = metadata.Created,
                Cache = metadata.Cache,
                Hash = metadata.Hash,
                Key = metadata.Key,
                LoaderSource = metadata.LoaderSource,
                Filters = metadata.Filters,
                LoaderType = metadata.LoaderType,
                MimeType = metadata.MimeType,
                Width = metadata.Width,
                Height = metadata.Height,
                DPR = metadata.DPR,
                FileLength = metadata.FileLength
            };

            //upload image metadata
            await ImageMetadatas.ReplaceOneAsync(Builders<ImageMetadataModel>.Filter.Where(x => x.Key == key), model, new ReplaceOptions() { IsUpsert = true });

            //upload transformed image
            await ImageBuffer.UploadFromStreamAsync(key, stream);
        }
    }
}
