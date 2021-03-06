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
        private readonly string _tableName;

        public ArticlesRepository(
            IDatabaseSettings databaseSettings,
            MongoDatabaseCRUD database, 
            IDateTime dataTime)
        {
            // configure database fields for articles
            ArticleConfiguration.Configure();
            
            _tableName = databaseSettings.TableName;
            _database = database;
            _dataTime = dataTime;
        }

        public Task<Article> GetById(Guid id, CancellationToken cancellationToken)
        {
            return _database.LoadRecordById<Article>(_tableName, id, cancellationToken);
        }

        public Task<IEnumerable<Article>> GetAll(CancellationToken cancellationToken)
        {
            return _database.LoadRecords<Article>(_tableName, cancellationToken);
        }

        public async Task Add(Article item, CancellationToken cancellationToken)
        {
            item.Created = _dataTime.Now;
            await _database.InsertRecord(_tableName, item, cancellationToken).ConfigureAwait(false);
        }

        public async Task Update(Guid id, Article item, CancellationToken cancellationToken)
        {
            item.LastModified = _dataTime.Now;
            await _database.UpsertRecord(_tableName, id, item, cancellationToken).ConfigureAwait(false);
        }

        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return _database.DeleteRecord<Article>(_tableName, id, cancellationToken);
        }
    }
}
