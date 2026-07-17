using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
