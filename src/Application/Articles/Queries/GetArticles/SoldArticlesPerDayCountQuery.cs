using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Queries.GetArticles
{
    public class SoldArticlesPerDayCountQuery : IRequest<Dictionary<DateTime, int>>
    {
    }

    public class SoldArticlesPerDayCountQueryHandler : IRequestHandler<SoldArticlesPerDayCountQuery, Dictionary<DateTime, int>>
    {
        private readonly IApplicationRepository<Article> _repository;

        public SoldArticlesPerDayCountQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<DateTime, int>> Handle(SoldArticlesPerDayCountQuery request, CancellationToken cancellationToken)
        {
            var articles = await _repository.GetAll(cancellationToken).ConfigureAwait(false);

            if (articles == null)
                throw new NotFoundException(nameof(articles));

            var articlesPerDay = articles
                .Where(x => x.Sold && x.SoldAt.HasValue)
                .GroupBy(x => new
                {
                    x.SoldAt.Value.Year,
                    x.SoldAt.Value.Month,
                    x.SoldAt.Value.Day
                })
                .Select(x => new
                {
                    Value = x.Count(),
                    x.Key.Year,
                    x.Key.Month,
                    x.Key.Day
                })
                .ToDictionary(article => new DateTime(article.Year, article.Month, article.Day),
                    article => article.Value);

            return articlesPerDay;
        }
    }
}
