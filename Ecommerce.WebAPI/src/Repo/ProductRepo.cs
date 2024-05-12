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
    public class ProductRepo : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Product> _products;
        private readonly DbSet<Order> _orders;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
            _products = _context.Products;
            _orders = _context.Orders;
        }

        public async Task<Product> AddAsync(Product entity)
        {

            _products.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _products.FindAsync(id);

            if (product == null)
                return false;
            _products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid productId)
        {
            return await _products.AnyAsync(p => p.Id == productId);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetMostPurchasedProductsAsync(int topNumber)
        {
            if (topNumber <= 0)
            {
                throw new ArgumentException("Invalid top number", nameof(topNumber));
            }

            var topProducts = await _context.OrderItems
                .Where(oi => oi.Order.Status == OrderStatus.Completed)
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

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync(Guid productId)
        {
            return await _products.Where(x => x.Id == productId).SelectMany(x => x.Images).ToListAsync();
        }

        public async Task<IEnumerable<Product>> ListAsync(ProductQueryOptions options)
        {
            var query = _context.Products.Include(p => p.Category).Include(p => p.Images).AsQueryable();

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
                    var propertyInfo = typeof(Product).GetProperty(options.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        query = options.SortOrder.ToLower() == "desc" ?
                            query.OrderByDescending(p => EF.Property<object>(p, options.SortBy)) :
                            query.OrderBy(p => EF.Property<object>(p, options.SortBy));
                    }
                }
                if (!string.IsNullOrEmpty(options.SortOrder)) //asc or desc
                {
                    switch (options.SortOrder.ToLower())
                    {
                        case "asc":
                            query = query.OrderBy(p => p.Title);
                            break;
                        case "desc":
                            query = query.OrderByDescending(p => p.Title);
                            break;
                        default:
                            // Handle invalid sort order (optional)
                            break;
                    }
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
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .SingleOrDefaultAsync(p => p.Id == entity.Id);
        }
    }
}