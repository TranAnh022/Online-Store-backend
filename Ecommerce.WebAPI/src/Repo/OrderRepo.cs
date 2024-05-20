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
    public class OrderRepo : BaseRepo<Order, QueryOptions>, IOrderRepository
    {

        private readonly DbSet<ProductSnapshot> _productSnapshots;
        private readonly DbSet<OrderItem> _orderItems;
        private readonly ICartRepository _cartRepository;
        public OrderRepo(AppDbContext context, ICartRepository cartRepository) : base(context)
        {

            _orderItems = context.OrderItems;
            _productSnapshots = context.ProductSnapshots;
            _cartRepository = cartRepository;
        }

        public async Task<Order> AddAsync(Order entity)
        {
            using (var transaction = _databaseContext.Database.BeginTransaction())
            {
                try
                {
                    await _data.AddAsync(entity);
                    await _databaseContext.SaveChangesAsync();

                    // Empty the cart associated with the user
                    var userId = entity.UserId;
                    var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                    if (cart != null)
                    {
                        cart.CartItems.Clear(); // Assuming CartItems is a list
                        await _databaseContext.SaveChangesAsync();
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
            await _databaseContext.SaveChangesAsync();
            return productSnapshot;
        }

        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId)
        {
            var order = await _data.Include(o => o.OrderItems).ThenInclude(oi => oi.ProductSnapshot).Where(x => x.UserId == userId).ToListAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> ListAsync(QueryOptions queryOptions)
        {
            var query = _data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryOptions.SortBy))
            {
                query = _data.Where(o => o.Status.ToString() == queryOptions.SortBy);
            }

            if (queryOptions.Page.HasValue && queryOptions.PageSize.HasValue)
            {
                query = query.Skip((queryOptions.Page.Value - 1) * queryOptions.PageSize.Value).Take(queryOptions.PageSize.Value);
            }
            return await query.Include(o => o.OrderItems).ThenInclude(oi => oi.ProductSnapshot).ToListAsync();
        }
    }
}