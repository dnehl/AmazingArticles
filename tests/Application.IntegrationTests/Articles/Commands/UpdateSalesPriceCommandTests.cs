using System;
using AmazingArticles.Application.Articles.Commands.DeleteArticle;
using NUnit.Framework;
using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticle;
using AmazingArticles.Application.Common.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal.Account;
using static AmazingArticles.Application.IntegrationTests.Testing;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice;

namespace AmazingArticles.Application.IntegrationTests.Articles.Commands
{
    public class UpdateArticleSalesPriceCommandTests : TestBase
    {
        [Test]
        public void ShouldUpdateArticleThrowsNotFound()
        {
            var command = new UpdateArticleSalesPriceCommand
            {
                SalesPrice = 1,
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

            var deleteCommand = new UpdateArticleSalesPriceCommand
            {
                Id = item.Id,
                SalesPrice = 400
                
            };

            await SendAsync(deleteCommand).ConfigureAwait(false);
            var updatedItem = FindArticle(t);
            updatedItem.Should().NotBeNull();
            updatedItem.SalesPrice.Should().Be(deleteCommand.SalesPrice);
        }
    }
}