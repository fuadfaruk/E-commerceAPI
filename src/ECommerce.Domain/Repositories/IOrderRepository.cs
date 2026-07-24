using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<Order?> GetWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
