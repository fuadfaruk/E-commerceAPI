using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public sealed class OrderRepository(ApplicationDbContext dbContext) : Repository<Order>(dbContext), IOrderRepository
    {
        public async Task<IReadOnlyList<Order>> GetOrdersForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await Dbcontext.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(cancellationToken);
        }

        public Task<Order?> GetWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return Dbcontext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }
    }
}
