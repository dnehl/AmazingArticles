using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice
{
    public class UpdateArticleSalesPriceCommand : IRequest
    {
        public Guid Id { get; set; }
        public double SalesPrice { get; set; }
    }

    public class UpdateArticleSalesPriceCommandHandler : IRequestHandler<UpdateArticleSalesPriceCommand>
    {
        private readonly IApplicationRepository<Article> _repository;
        public UpdateArticleSalesPriceCommandHandler(IApplicationRepository<Article> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateArticleSalesPriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id, cancellationToken).ConfigureAwait(false);

            if (entity == null)
                throw new NotFoundException(nameof(Article), request.Id);

            entity.SalesPrice = request.SalesPrice;
            await _repository.Update(request.Id, entity, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
