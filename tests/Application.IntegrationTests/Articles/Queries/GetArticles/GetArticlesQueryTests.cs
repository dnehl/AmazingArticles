using System.Linq;
using System.Threading.Tasks;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using FluentAssertions;
using NUnit.Framework;

namespace AmazingArticles.Application.IntegrationTests.Articles.Queries.GetArticles
{
    public class GetArticlesQueryTests
    {
        [Test]
        public async Task ShouldReturnsItems()
        {
            var query = new GetArticlesQuery();
            var result = await Testing.SendAsync(query);
            result.Count().Should().BeGreaterThan(0);
        }
    }
}