using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Queries.Revenues
{
    public class RevenuesPerDayQuery : IRequest<Dictionary<DateTime, double>>
    {
    }

    public class RevenuesPerDayQueryHandler : IRequestHandler<RevenuesPerDayQuery, Dictionary<DateTime, double>>
    {
        private readonly IApplicationRepository<Article> _repository;

        public RevenuesPerDayQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<DateTime, double>> Handle(RevenuesPerDayQuery request, CancellationToken cancellationToken)
        {
            var articles = await _repository.GetAll(cancellationToken).ConfigureAwait(false);

            if (articles == null)
                throw new NotFoundException(nameof(Article));
            return articles
                .GroupBy(x => new
                {
                    Year = x.Created.Year,
                    Month = x.Created.Month,
                    Day = x.Created.Day
                })
                .Select(x => new
                {
                    TotalRevenue = x.Sum(x => x.SalesPrice),
                    Year = x.Key.Year,
                    Month = x.Key.Month,
                    Day = x.Key.Day
                })
                .ToDictionary(article => new DateTime(article.Year, article.Month, article.Day),
                    article => article.TotalRevenue);
        }
    }
}
