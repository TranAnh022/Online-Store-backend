using System.Reflection;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ProductRepo : BaseRepo<Product, ProductQueryOptions>, IProductRepository
    {
        private readonly DbSet<Order> _orders;
        public ProductRepo(AppDbContext context) : base(context)
        {
            _orders = context.Orders;
        }


        public async Task<IEnumerable<Product>> GetMostPurchasedProductsAsync(int topNumber)
        {
            if (topNumber <= 0)
            {
                throw new ArgumentException("Invalid top number", nameof(topNumber));
            }

            var topProducts = await _orders
                .Include(o => o.OrderItems) // Include the OrderItems related to each Order
                .Where(o => o.Status == OrderStatus.Completed)
                .SelectMany(o => o.OrderItems) // Flatten the OrderItems from multiple Orders into a single sequence
                .GroupBy(oi => new { oi.ProductSnapshot.ProductId, oi.ProductSnapshot.Title })
                .Select(g => new
            {
                ProductId = g.Key.ProductId,
                Title = g.Key.Title,
                TotalQuantityPurchased = g.Sum(oi => oi.Quantity)
            })
                .OrderByDescending(x => x.TotalQuantityPurchased)
                .Take(topNumber)
                .ToListAsync();

            // Mapping the anonymous type to Product entity
            var products = topProducts.Select(tp => new Product
            {
                Id = tp.ProductId,
                Title = tp.Title,

            });

            return products;

        }


        public async Task<IEnumerable<Product>> ListAsync(ProductQueryOptions options)
        {
            var query = _data.Include(p => p.Category).Include(p => p.Images).AsQueryable();

            // Apply filters if ProductQueryOptions is not null
            if (options != null)
            {
                // Filter by search title
                if (!string.IsNullOrEmpty(options.Title))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(options.Title.ToLower()));
                }
                // Filter by price range
                if (options.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= options.MinPrice.Value);
                }

                if (options.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= options.MaxPrice.Value);
                }

                // Sorting
                if (!string.IsNullOrEmpty(options.SortBy))
                {
                    query = query.Where(p => p.Category.ToString().ToLower() == options.SortBy.ToLower());
                }

                // Pagination
                if (options.Page.HasValue && options.PageSize.HasValue)
                    query = query.Skip(options.Page.Value - 1).Take(options.PageSize.Value);
            }

            // Execute the query
            var products = await query.ToListAsync();
            return products;
        }


        public async Task<Product> UpdateAsync(Product entity)
        {
            _data.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
            return await _data
            .Include(p => p.Category)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == entity.Id);
        }
    }
}