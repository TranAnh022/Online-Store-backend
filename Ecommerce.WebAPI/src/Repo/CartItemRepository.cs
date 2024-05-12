using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class CartItemRepository : IBaseRepository<CartItem, QueryOptions>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<CartItem> _cartItems;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
            _cartItems = context.CartsItem;
        }

        public async Task<CartItem> AddAsync(CartItem entity)
        {
            _cartItems.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _cartItems.FindAsync(id);
            if (item == null) return false;
            _cartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CartItem>> ListAsync(QueryOptions options)
        {
            IQueryable<CartItem> query = _cartItems.Include(ci => ci.Product);

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(ci => ci.Product!.Title!.Contains(options.Search));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(options.SortBy))
            {
                var descending = options.SortOrder == "desc";
                switch (options.SortBy.ToLower())
                {
                    case "Quantity":
                        query = descending ? query.OrderByDescending(ci => ci.Quantity) : query.OrderBy(ci => ci.Quantity);
                        break;
                    case "ProductTitle":
                        query = descending ? query.OrderByDescending(ci => ci.Product!.Title) : query.OrderBy(ci => ci.Product!.Title);
                        break;
                    default:
                        throw new ArgumentException("Invalid sort column");
                }
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<CartItem> GetByIdAsync(Guid id)
        {
            return await _cartItems
                .Include(ci => ci.Product)
                .SingleOrDefaultAsync(ci => ci.Id == id) ?? throw new InvalidOperationException("Cart Items not found");
        }

        public async Task<CartItem> UpdateAsync(CartItem entity)
        {
            var existingCartItem = await _cartItems.FindAsync(entity.Id) ?? throw new KeyNotFoundException("Cart item not found.");
            existingCartItem.Quantity = entity.Quantity;
            _cartItems.Update(existingCartItem);
            await _context.SaveChangesAsync();
            return existingCartItem;
        }
    }
}