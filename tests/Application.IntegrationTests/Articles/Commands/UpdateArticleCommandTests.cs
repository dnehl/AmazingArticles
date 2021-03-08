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

namespace AmazingArticles.Application.IntegrationTests.Articles.Commands
{
    public class UpdateArticleCommandTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateArticleCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldUpdateArticleThrowsNotFound()
        {
            var command = new UpdateArticleCommand
            {
                ArticleNumber = "Foo",
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

            var deleteCommand = new UpdateArticleCommand
            {
                Id = item.Id,
                ArticleNumber = "Foo²"
                
            };

            await SendAsync(deleteCommand).ConfigureAwait(false);
            var updatedItem = FindArticle(t);
            updatedItem.Should().NotBeNull();
            updatedItem.ArticleNumber.Should().Be(deleteCommand.ArticleNumber);
        }
    }
}