using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Commands.UpdateArticleSold
{
    public class UpdateArticleSoldCommand : IRequest<Article>
    {
        public Guid Id { get; set; }
    }

    public class UpdateArticleSoldCommandHandler : IRequestHandler<UpdateArticleSoldCommand, Article>
    {
        private readonly IApplicationRepository<Article> _repository;
        private readonly IDateTime _dateTime;
        public UpdateArticleSoldCommandHandler(IApplicationRepository<Article> repository, IDateTime dateTime)
        {
            _repository = repository;
            _dateTime = dateTime;
        }

        public async Task<Article> Handle(UpdateArticleSoldCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity == null)
                throw new NotFoundException(nameof(Article), request.Id);

            if (entity.Sold)
                throw new ArticleAlreadySoldException(nameof(entity.ArticleNumber), request.Id);

            entity.SoldAt = _dateTime.Now;
            entity.Sold = true;
            await _repository.Update(request.Id, entity, cancellationToken).ConfigureAwait(false);

            var entity2 = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);
            return entity;
        }
    }
}
