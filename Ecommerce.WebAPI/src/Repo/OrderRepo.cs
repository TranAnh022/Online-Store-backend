using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class OrderRepo : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Order> _orders;
        private readonly DbSet<ProductSnapshot> _productSnapshots;
        private readonly DbSet<OrderItem> _orderItems;
        private readonly ICartRepository _cartRepository;
        public OrderRepo(AppDbContext context, ICartRepository cartRepository)
        {
            _context = context;
            _orders = _context.Orders;
            _orderItems = _context.OrderItems;
            _productSnapshots = _context.ProductSnapshots;
            _cartRepository = cartRepository;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Orders.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    // Empty the cart associated with the user
                    var userId = entity.UserId;
                    var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                    if (cart != null)
                    {
                        cart.CartItems.Clear(); // Assuming CartItems is a list
                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                    return entity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ProductSnapshot> AddProductSnapshotAsync(ProductSnapshot productSnapshot)
        {
            _productSnapshots.Add(productSnapshot);
            var result = await _context.SaveChangesAsync() > 0;
            return productSnapshot;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _orders.FindAsync(id);
            if (order == null)
                return false;
            _orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _orders.FindAsync(id) ?? throw new ArgumentException("Order not found", nameof(id)); ;
        }

        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId)
        {
            var order = await _orders.Include(o => o.OrderItems).ThenInclude(oi => oi.ProductSnapshot).Where(x => x.UserId == userId).ToListAsync();
            return order;
        }


        public async Task<IEnumerable<Order>> ListAsync(QueryOptions queryOptions)
        {
            var query = _orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryOptions.SortBy))
            {
                query = _orders.Where(o => o.Status.ToString() == queryOptions.SortBy);
            }

            if (queryOptions.Page.HasValue && queryOptions.PageSize.HasValue)
            {
                query = query.Skip((queryOptions.Page.Value - 1) * queryOptions.PageSize.Value).Take(queryOptions.PageSize.Value);
            }
            return await query.Include(o=>o.OrderItems).ThenInclude(oi=>oi.ProductSnapshot).ToListAsync();
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await _orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}