using ImageWizard.ImageFormats.Base;
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

namespace ImageWizard.MongoDB.ImageCaches
{
    /// <summary>
    /// MongoDBCache
    /// </summary>
    public class MongoDBCache : IImageCache
    {
        public MongoDBCache(IOptions<MongoDBCacheSettings> settings)
        {
            Client = new Mongo.MongoClient(new Mongo.MongoClientSettings() { Server = new Mongo.MongoServerAddress(settings.Value.Hostname) });

            Database = Client.GetDatabase(settings.Value.Database);

            ImageMetadatas = Database.GetCollection<ImageMetadataModel>("ImageMetadata");
            ImageBuffer = new GridFSBucket(Database, new GridFSBucketOptions() { BucketName = "ImageBuffer" });

            //create index
            ImageMetadatas.Indexes.CreateOne(new CreateIndexModel<ImageMetadataModel>(new IndexKeysDefinitionBuilder<ImageMetadataModel>().Ascending(x => x.Signature)));
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

        public async Task<CachedImage> ReadAsync(string key)
        {
            ImageMetadataModel foundMetadata = await ImageMetadatas
                                                            .AsQueryable()
                                                            .FirstOrDefaultAsync(x => x.Signature == key);

            if(foundMetadata == null)
            {
                return null;
            }

            byte[] data = await ImageBuffer.DownloadAsBytesByNameAsync(key);

            CachedImage cachedImage = new CachedImage();
            cachedImage.Metadata = foundMetadata;
            cachedImage.Data = data;

            return cachedImage;
        }

        public async Task WriteAsync(string key, CachedImage cachedImage)
        {
            ImageMetadataModel model = new ImageMetadataModel()
            {
                Signature = cachedImage.Metadata.Signature,
                MimeType = cachedImage.Metadata.MimeType,
                Url = cachedImage.Metadata.Url
            };

            //upload image metadata
            await ImageMetadatas.ReplaceOneAsync(Builders<ImageMetadataModel>.Filter.Where(x => x.Signature == key), model, new UpdateOptions() { IsUpsert = true });

            //upload transformed image
            await ImageBuffer.UploadFromBytesAsync(key, cachedImage.Data);
        }
    }
}
