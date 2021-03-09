using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSold;
using AmazingArticles.Application.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static AmazingArticles.Application.IntegrationTests.Testing;

namespace AmazingArticles.Application.IntegrationTests.Articles.Commands
{
    public class UpdateArticleSoldCommandTests : TestBase
    {
        [Test]
        public void ShouldUpdateArticleThrowsNotFound()
        {
            var command = new UpdateArticleSoldCommand()
            {
                Id = Guid.Empty
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdateItem()
        {
            var articleCommand = new CreateArticleCommand
            {
                SalesPrice = 400,
                ArticleNumber = "Foo"
            };

            var t = await SendAsync(articleCommand).ConfigureAwait(false);

            var item = FindArticle(t);
            item.Should().NotBeNull();
            item.Id.Should().Be(t);

            var updateCommand = new UpdateArticleSoldCommand
            {
                Id = item.Id
            };

            await SendAsync(updateCommand).ConfigureAwait(false);
            var updatedItem = FindArticle(t);
            updatedItem.Should().NotBeNull();
            updatedItem.Sold.Should().Be(true);
        }

        [Test]
        public async Task ShouldUpdateItemThrowsAlreadySoldException()
        {
            var articleCommand = new CreateArticleCommand
            {
                SalesPrice = 400,
                ArticleNumber = "Foo"
            };

            var t = await SendAsync(articleCommand).ConfigureAwait(false);

            var item = FindArticle(t);
            item.Should().NotBeNull();
            item.Id.Should().Be(t);

            var updateCommand = new UpdateArticleSoldCommand
            {
                Id = item.Id
            };

            await SendAsync(updateCommand).ConfigureAwait(false);
            var updatedItem = FindArticle(t);
            updatedItem.Should().NotBeNull();
            updatedItem.Sold.Should().Be(true);


            FluentActions.Invoking(() =>
                SendAsync(updateCommand)).Should().Throw<ArticleAlreadySoldException>();
        }
    }
}