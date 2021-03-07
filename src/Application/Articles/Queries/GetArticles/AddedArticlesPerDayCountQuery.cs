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
    public class AddedArticlesPerDayCountQuery : IRequest<Dictionary<DateTime, int>>
    {
    }

    public class AddedArticlesPerDayCountQueryHandler : IRequestHandler<AddedArticlesPerDayCountQuery, Dictionary<DateTime, int>>
    {
        private readonly IApplicationRepository<Article> _repository;

        public AddedArticlesPerDayCountQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<DateTime, int>> Handle(AddedArticlesPerDayCountQuery request, CancellationToken cancellationToken)
        {
            var articles = await _repository.GetAll(cancellationToken).ConfigureAwait(false);

            if (articles == null)
                throw new NotFoundException(nameof(articles));

            var articlesPerDay = articles
                .GroupBy(x => new
                {
                    Year = x.Created.Year,
                    Month = x.Created.Month,
                    Day = x.Created.Day
                })
                .Select(x => new
                {
                    Value = x.Count(),
                    Year = x.Key.Year,
                    Month = x.Key.Month,
                    Day = x.Key.Day
                })
                .ToDictionary(article => new DateTime(article.Year, article.Month, article.Day),
                    article => article.Value);

            return articlesPerDay;
        }
    }
}
