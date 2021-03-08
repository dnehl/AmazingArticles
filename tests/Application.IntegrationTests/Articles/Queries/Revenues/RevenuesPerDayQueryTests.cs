using AmazingArticles.Application.Articles.Queries.Revenues;
using AmazingArticles.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AmazingArticles.Application.IntegrationTests.Testing;

namespace AmazingArticles.Application.IntegrationTests.Articles.Queries.Revenues
{
    public class RevenuesPerDayQueryTests
    {
        [Test]
        public async Task ShouldReturnsItems()
        {
            ResetArticles();
            var articles = new List<Article>
            {
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 08),
                    SalesPrice = 400,
                    ArticleNumber = "Foo1"
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 08),
                    SalesPrice = 300,
                    ArticleNumber = "Foo2"
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 08),
                    SalesPrice = 400,
                    ArticleNumber = "Foo4"
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 07),
                    SalesPrice = 400,
                    ArticleNumber = "Foo4"
                }
            };

            foreach (var article in articles)
                AddArticle(article);
            

            var query = new RevenuesPerDayQuery();
            var result = await SendAsync(query);
            result.Count().Should().BeGreaterThan(0);
            result[new DateTime(2021, 03, 08)].Should().Be(1100);
            result[new DateTime(2021, 03, 07)].Should().Be(400);
        }
    }
}
