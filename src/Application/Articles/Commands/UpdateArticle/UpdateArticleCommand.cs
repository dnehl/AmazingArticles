using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommand : IRequest
    {
        public Guid Id { get; set; }

        public string ArticleNumber { get; set; }

        public double SalesPrice { get; set; }
    }

    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
    {
        private readonly IApplicationRepository<Article> _repository;
        public UpdateArticleCommandHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity == null)
                throw new NotFoundException(nameof(Article), request.Id);

            entity.ArticleNumber = request.ArticleNumber;
            entity.SalesPrice = request.SalesPrice;

            await _repository.Update(request.Id, entity, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
