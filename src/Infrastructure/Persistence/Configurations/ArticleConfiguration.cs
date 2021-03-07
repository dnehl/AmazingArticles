using AmazingArticles.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AmazingArticles.Infrastructure.Persistence.Configurations
{
    public static class ArticleConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Article>(a =>
            {
                a.MapIdProperty(x => x.Id).SetIdGenerator(new GuidGenerator());
                a.MapProperty(x => x.ArticleNumber).SetElementName("ArticleNumber");
                a.MapProperty(x => x.SalesPrice).SetElementName("Foo");
            });
        }
    }
}