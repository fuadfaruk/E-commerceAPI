using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public sealed class CustomerRepository(ApplicationDbContext dbContext) : Repository<Customer>(dbContext), ICustomerRepository
    {
        public Task<Customer?> GetByIdentityIdAsyc(string identityId, CancellationToken cancellationToken = default)
        {
            return Dbcontext.Customers.FirstOrDefaultAsync(customer => customer.UserIdentityId == identityId, cancellationToken);
        }
    }
}
