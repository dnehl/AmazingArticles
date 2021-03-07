using AmazingArticles.Domain.Common;
using AmazingArticles.Domain.Entities;

namespace AmazingArticles.Domain.Events
{
    public class ArticleCreatedEvent : DomainEvent
    {
        public ArticleCreatedEvent(Article article) => Article = article;
        public Article Article { get; }
    }
}
