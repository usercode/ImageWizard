using ImageWizard.ImageStorages;
using ImageWizard.Services.Types;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System;
using System.Threading.Tasks;
using Mongo=MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using ImageWizard.MongoDB.Models;
using ImageWizard.Types;
using ImageWizard.Core.Types;
using System.IO;

namespace ImageWizard.MongoDB.ImageCaches
{
    /// <summary>
    /// MongoDBCache
    /// </summary>
    public class MongoDBCache : IImageCache
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

        public async Task<ICachedImage> ReadAsync(string key)
        {
            ImageMetadataModel foundMetadata = await ImageMetadatas
                                                            .AsQueryable()
                                                            .FirstOrDefaultAsync(x => x.Key == key);

            if(foundMetadata == null)
            {
                return null;
            }

            CachedImage cachedImage = new CachedImage(foundMetadata, async () => await ImageBuffer.OpenDownloadStreamByNameAsync(key));

            return cachedImage;
        }

        public async Task WriteAsync(string key, ICachedImage cachedImage)
        {
            ImageMetadataModel model = new ImageMetadataModel()
            {
                Created = cachedImage.Metadata.Created,
                Cache = cachedImage.Metadata.Cache,
                Hash = cachedImage.Metadata.Hash,
                Key = cachedImage.Metadata.Key,
                LoaderSource = cachedImage.Metadata.LoaderSource,
                Filters = cachedImage.Metadata.Filters,
                LoaderType = cachedImage.Metadata.LoaderType,
                MimeType = cachedImage.Metadata.MimeType,
                Width = cachedImage.Metadata.Width,
                Height = cachedImage.Metadata.Height,
                DPR = cachedImage.Metadata.DPR,
                FileLength = cachedImage.Metadata.FileLength
            };

            //upload image metadata
            await ImageMetadatas.ReplaceOneAsync(Builders<ImageMetadataModel>.Filter.Where(x => x.Key == key), model, new ReplaceOptions() { IsUpsert = true });

            using (Stream cachedImageStream = await cachedImage.OpenReadAsync())
            {
                //upload transformed image
                await ImageBuffer.UploadFromStreamAsync(key, cachedImageStream);
            }
        }
    }
}
