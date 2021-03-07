using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Common.Interfaces
{
    public interface IApplicationRepository<TItem>
    {
        Task<TItem> GetById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<TItem>> GetAll(CancellationToken cancellationToken);
        Task Add(TItem item, CancellationToken cancellationToken);
        Task Update(Guid id, TItem item, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
