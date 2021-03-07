using AmazingArticles.Application.Common.Models;
using AmazingArticles.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Articles.EventHandlers
{
    public class ArticleCreatedEventHandler : INotificationHandler<DomainEventNotification<ArticleCreatedEvent>>
    {
        private readonly ILogger<ArticleCreatedEventHandler> _logger;

        public ArticleCreatedEventHandler(ILogger<ArticleCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<ArticleCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("AmazingArticles Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
