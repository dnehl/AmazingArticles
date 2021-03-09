using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Queries.Revenues
{
    public class RevenuesPerArticlesQuery : IRequest<Dictionary<string, double>>
    {
    }

    public class RevenuesPerArticlesQueryHandler : IRequestHandler<RevenuesPerArticlesQuery, Dictionary<string, double>>
    {
        private readonly IApplicationRepository<Article> _repository;

        public RevenuesPerArticlesQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<string, double>> Handle(RevenuesPerArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = await _repository.GetAll(cancellationToken).ConfigureAwait(false);

            if (articles == null)
                throw new NotFoundException(nameof(Article));
            return articles
                .Where(x => x.Sold)
                .GroupBy(x => new
                {
                    x.ArticleNumber
                })
                .Select(x => new
                {
                    TotalRevenue = x.Sum(x => x.SalesPrice),
                    x.Key.ArticleNumber
                })
                .ToDictionary(article => article.ArticleNumber,
                    article => article.TotalRevenue);
        }
    }
}
