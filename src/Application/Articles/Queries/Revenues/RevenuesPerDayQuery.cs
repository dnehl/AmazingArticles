using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                .Where(x => x.Sold && x.SoldAt.HasValue)
                .GroupBy(x => new
                {
                    x.SoldAt.Value.Year,
                    x.SoldAt.Value.Month,
                    x.SoldAt.Value.Day
                })
                .Select(x => new
                {
                    TotalRevenue = x.Sum(x => x.SalesPrice),
                    x.Key.Year,
                    x.Key.Month,
                    x.Key.Day
                })
                .ToDictionary(article => new DateTime(article.Year, article.Month, article.Day),
                    article => article.TotalRevenue);
        }
    }
}
