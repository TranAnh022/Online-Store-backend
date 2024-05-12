using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class OrderItemRepository : IBaseRepository<OrderItem, QueryOptions>
    {
        private readonly AppDbContext _context;

        private readonly DbSet<OrderItem> _orderItems;
        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
            _orderItems = _context.OrderItems;
        }

        public async Task<OrderItem> AddAsync(OrderItem entity)
        {
            _orderItems.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _orderItems.FindAsync(id);
            if (item == null) return false;
            _orderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderItem> GetByIdAsync(Guid id)
        {
            return await _orderItems.FindAsync(id) ?? throw new ArgumentException("OrderItem not found", nameof(id)); ;
        }

        public async Task<IEnumerable<OrderItem>> ListAsync(QueryOptions queryOptions)
        {
            var query = _orderItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryOptions.SortBy))
            {
                switch (queryOptions.SortBy.ToLower())
                {
                    case "price":
                        query = queryOptions.SortOrder == "asc" ? query.OrderBy(oi => oi.Price) : query.OrderByDescending(p => p.Price);
                        break;
                    case "quantity":
                        query = queryOptions.SortOrder == "asc" ? query.OrderBy(oi => oi.Quantity) : query.OrderByDescending(oi => oi.Quantity);
                        break;
                    default:
                        break;
                }
            }

            if (queryOptions.Page.HasValue && queryOptions.PageSize.HasValue)
            {
                query = query.Skip((queryOptions.Page.Value - 1) * queryOptions.PageSize.Value).Take(queryOptions.PageSize.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<OrderItem> UpdateAsync(OrderItem entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}