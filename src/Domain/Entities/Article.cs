using AmazingArticles.Domain.Common;
using System;

namespace AmazingArticles.Domain.Entities
{
    public class Article : AuditableEntity
    {
        public Guid Id { get; set; }
        public string ArticleNumber { get; set; }
        public double SalesPrice { get; set; }
    }
}
