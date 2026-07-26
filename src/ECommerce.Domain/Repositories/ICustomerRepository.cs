using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByIdentityIdAsyc(string identityId, CancellationToken cancellationToken = default);
    }
}
