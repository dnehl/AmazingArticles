using System;
using AmazingArticles.Domain.Common;
using System.Collections.Generic;

namespace AmazingArticles.Domain.Entities
{
    public class Article : AuditableEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        public string ArticleNumber { get; set; }
        public double SalesPrice { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new();
    }
}
