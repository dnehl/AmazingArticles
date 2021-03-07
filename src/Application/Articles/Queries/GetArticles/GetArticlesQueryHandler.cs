using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Queries.GetArticles
{
    public class GetArticlesQuery : IRequest<IEnumerable<Article>>
    {
    }


    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, IEnumerable<Article>>
    {
        private readonly IApplicationRepository<Article> _repository;

        public GetArticlesQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Article>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAll(cancellationToken);
            
        }
    }
}
