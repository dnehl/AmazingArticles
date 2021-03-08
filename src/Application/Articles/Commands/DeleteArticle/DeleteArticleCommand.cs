using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Commands.DeleteArticle
{

    public class DeleteArticleCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IApplicationRepository<Article> _repository;

        public DeleteArticleCommandHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity == null)
                throw new NotFoundException(nameof(Article), request.Id);

            await _repository.Delete(entity.Id, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}


