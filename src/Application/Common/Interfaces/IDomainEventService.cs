using AmazingArticles.Domain.Common;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
