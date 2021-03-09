using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using AmazingArticles.Application.Articles.Queries.Revenues;
using AmazingArticles.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static AmazingArticles.Application.IntegrationTests.Testing;

namespace AmazingArticles.Application.IntegrationTests.Articles.Queries.Revenues
{
    public class RevenuesPerArticlesQueryTests
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
                    ArticleNumber = "Foo1",
                    Sold = true,
                    SoldAt = new DateTime(2021, 03, 08)
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 08),
                    SalesPrice = 300,
                    ArticleNumber = "Foo2",
                    Sold = true,
                    SoldAt = new DateTime(2021, 03, 08)
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 08),
                    SalesPrice = 400,
                    ArticleNumber = "Foo4",
                    Sold = true,
                    SoldAt = new DateTime(2021, 03, 08)
                },
                new Article()
                {
                    Id = Guid.NewGuid(),
                    Created = new DateTime(2021, 03, 07),
                    SalesPrice = 400,
                    ArticleNumber = "Foo4",
                    Sold = true,
                    SoldAt = new DateTime(2021, 03, 07)
                }
            };

            foreach (var article in articles)
                AddArticle(article);
            

            var query = new RevenuesPerArticlesQuery();
            var result = await SendAsync(query);
            result.Count().Should().BeGreaterThan(0);
            result["Foo1"].Should().Be(400);
            result["Foo4"].Should().Be(800);
        }
    }
}
