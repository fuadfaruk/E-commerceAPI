using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
