using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using AmazingArticles.Infrastructure.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Infrastructure.Persistence
{
    public class ArticlesRepository : IApplicationRepository<Article>
    {
        private readonly IDateTime _dataTime;
        private readonly MongoDatabaseCRUD _database;
        private const string TableName = "Articles";

        public ArticlesRepository(
            MongoDatabaseCRUD database, 
            IDateTime dataTime)
        {
            // configure database fields for articles
            ArticleConfiguration.Configure();

            _database = database;
            _dataTime = dataTime;
        }

        public Task<Article> GetById(Guid id, CancellationToken cancellationToken)
        {
            return _database.LoadRecordById<Article>(TableName, id, cancellationToken);
        }

        public Task<IEnumerable<Article>> GetAll(CancellationToken cancellationToken)
        {
            return _database.LoadRecords<Article>(TableName, cancellationToken);
        }

        public async Task Add(Article item, CancellationToken cancellationToken)
        {
            item.Created = _dataTime.Now;
            await _database.InsertRecord(TableName, item, cancellationToken).ConfigureAwait(false);
        }

        public async Task Update(Guid id, Article item, CancellationToken cancellationToken)
        {
            item.LastModified = _dataTime.Now;
            await _database.UpsertRecord(TableName, id, item, cancellationToken).ConfigureAwait(false);
        }

        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return _database.DeleteRecord<Article>(TableName, id, cancellationToken);
        }
    }
}
