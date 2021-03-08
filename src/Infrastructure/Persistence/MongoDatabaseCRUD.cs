using AmazingArticles.Infrastructure.Persistence.Configurations;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Infrastructure.Persistence
{
    public class MongoDatabaseCRUD
    {
        private readonly IMongoDatabase _database;

        public MongoDatabaseCRUD(IDatabaseSettings databaseSettings)
        {
            var settings = MongoClientSettings.FromUrl(
                new MongoUrl(databaseSettings.ConnectionString)
            );
            settings.SslSettings =
                new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
        }

        public async Task InsertRecord<T>(string tableName, T record, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<T>(tableName);
            await collection.InsertOneAsync(record, new InsertOneOptions(), cancellationToken).ConfigureAwait(false);
        }

        public async Task<T> LoadRecordById<T>(string tableName, Guid id, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<T>(tableName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var record = await collection
                .FindAsync(filter, new FindOptions<T>(), cancellationToken)
                .ConfigureAwait(false);

            return record.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> LoadRecords<T>(string tableName, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<T>(tableName);
            var records = await collection
                .FindAsync(new BsonDocument(), new FindOptions<T>(), cancellationToken)
                .ConfigureAwait(false);

            return records.ToEnumerable();
        }

        public async Task UpsertRecord<T>(string tableName, Guid id, T record, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<T>(tableName);
            var document = new BsonDocument("_id", new BsonBinaryData(id, GuidRepresentation.Standard));
            await collection
                .ReplaceOneAsync(document, record, new ReplaceOptions {IsUpsert = true}, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteRecord<T>(string tableName, Guid id, CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<T>(tableName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await collection.DeleteOneAsync(filter, cancellationToken).ConfigureAwait(false);
        }
    }
}