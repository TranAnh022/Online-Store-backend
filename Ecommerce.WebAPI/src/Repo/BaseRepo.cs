using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class BaseRepo<T, TQueryOptions> : IBaseRepository<T, TQueryOptions>
        where T : BaseEntity
        where TQueryOptions : QueryOptions
    {
        protected readonly DbSet<T> _data;
        protected readonly AppDbContext _databaseContext;

        public BaseRepo(AppDbContext databaseContext)
        {
            _databaseContext = databaseContext;
            _data = _databaseContext.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _data.Add(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var find = await _data.FindAsync(id);
            _data.Remove(find);
            _databaseContext.SaveChanges();
            return true;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return  await _data.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ListAsync(TQueryOptions options)
        {
            return await _data.Skip(options.Page.Value - 1).Take(options.PageSize.Value).ToListAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _data.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> ExistsAsync(Guid Id)
        {
            return await _data.AnyAsync(u => u.Id == Id);
        }

    }
}
