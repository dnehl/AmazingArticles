using System;
using AmazingArticles.Application.Articles.Commands.DeleteArticle;
using NUnit.Framework;
using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Common.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal.Account;
using static AmazingArticles.Application.IntegrationTests.Testing;

namespace AmazingArticles.Application.IntegrationTests.Articles.Commands
{
    public class DeleteArticleCommandTests : TestBase
    {
        [Test]
        public void ShouldDeleteArticleThrowsNotFound()
        {
            var deleteCommand = new DeleteArticleCommand
            {
                Id = Guid.Empty
            };

            FluentActions.Invoking(() =>
                SendAsync(deleteCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteItem()
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

            var deleteCommand = new DeleteArticleCommand
            {
                Id = item.Id
            };

            await SendAsync(deleteCommand).ConfigureAwait(false);
            item = FindArticle(t);
            item.Should().BeNull();
        }
    }
}