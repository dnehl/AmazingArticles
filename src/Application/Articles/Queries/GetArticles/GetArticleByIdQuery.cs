using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Queries.GetArticles
{
    public class GetArticleByIdQuery : IRequest<Article>
    {
        public Guid Id { get; set; }
    }


    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, Article>
    {
        private readonly IApplicationRepository<Article> _repository;

        public GetArticleByIdQueryHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Article> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (article == null)
                throw new NotFoundException(nameof(article), request.Id);
            return article;
        }
    }
}
