using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using AmazingArticles.Domain.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommand : IRequest<Guid>
    {
        public string ArticleNumber { get; set; }
        public double SalesPrice { get; set; }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
    {
        private readonly IApplicationRepository<Article> _repository;

        public CreateTodoItemCommandHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var entity = new Article
            {
                ArticleNumber = request.ArticleNumber,
                SalesPrice = request.SalesPrice
            };

            entity.DomainEvents.Add(new ArticleCreatedEvent(entity));

            await _repository.Add(entity, cancellationToken).ConfigureAwait(false);
            return entity.Id;
        }
    }
}
