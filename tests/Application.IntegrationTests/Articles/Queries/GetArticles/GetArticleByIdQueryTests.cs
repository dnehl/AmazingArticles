using System;
using System.Linq;
using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static AmazingArticles.Application.IntegrationTests.Testing;

namespace AmazingArticles.Application.IntegrationTests.Articles.Queries.GetArticles
{
    public class GetArticleByIdQueryTests
    {
        [Test]
        public void ThrowsNotFoundException()
        {
            var query = new GetArticleByIdQuery { Id = Guid.Empty };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldReturnsItem()
        {
            var article = new Article()
            {
                Id = Guid.NewGuid(),
                Created = new DateTime(2021, 03, 08),
                SalesPrice = 400,
                ArticleNumber = "Foo1",
                Sold = true,
                SoldAt = new DateTime(2021, 03, 08)
            };

            AddArticle(article);

            var query = new GetArticleByIdQuery{Id = article.Id};
            var result = await Testing.SendAsync(query);
            result.Should().NotBeNull();
            result.Id.Should().Be(article.Id);
        }
    }
}