using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Common.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;
using static AmazingArticles.Application.IntegrationTests.Testing;
namespace AmazingArticles.Application.IntegrationTests.Articles.Commands
{
    public class CreateArticleCommandTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateArticleCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateArticle()
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

        }
    }
}
