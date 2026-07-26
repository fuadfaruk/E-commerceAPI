using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public sealed class ProductRepository(ApplicationDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
    {
        public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await Dbcontext.Products.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            var idList = ids.ToList();
            return await Dbcontext.Products.Where(p => idList.Contains(p.Id)).ToListAsync(cancellationToken);
        }
    }
}
