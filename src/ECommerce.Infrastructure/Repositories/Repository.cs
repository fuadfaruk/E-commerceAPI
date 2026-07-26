using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T>
        where T : Entity
    {
        protected readonly ApplicationDbContext Dbcontext = dbContext;
        public virtual Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Dbcontext.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Dbcontext.Set<T>().AddAsync(entity, cancellationToken).AsTask();
        }


        public void Remove(T entity)
        {
            Dbcontext.Set<T>().Remove(entity);
        }
    }
}
