using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
            _categories = _context.Categories;
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _categories.FindAsync(id);
            if (category == null)
                return false;
            _categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _categories.FindAsync(id) ?? throw new ArgumentException("Category not found", nameof(id)); ;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, ProductQueryOptions queryOptions)
        {
            var products = _context.Products.Where(p => p.CategoryId == categoryId);
            // Sorting
            if (!string.IsNullOrWhiteSpace(queryOptions.SortBy))
            {
                switch (queryOptions.SortBy.ToLower())
                {
                    case "name":
                        products = queryOptions.SortOrder == "asc" ? products.OrderBy(p => p.Title) : products.OrderByDescending(p => p.Title);
                        break;
                    case "price":
                        products = queryOptions.SortOrder == "asc" ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                        break;
                    default:
                        break;
                }
            }
            // Pagination
            if (queryOptions.Page.HasValue && queryOptions.PageSize.HasValue)
            {
                products = products.Skip((queryOptions.Page.Value - 1) * queryOptions.PageSize.Value).Take(queryOptions.PageSize.Value);
            }
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Category>> ListAsync(QueryOptions options)
        {
            var query = _categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(c => c.Name!.Contains(options.Search));
            }

            if (!string.IsNullOrWhiteSpace(options.SortBy) && options.SortBy.ToLower() == "name")
            {
                query = options.SortOrder == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            var category = await _context.Categories.FindAsync(entity.Id);
            if (category == null) return null!;

            category.Name = entity.Name;
            category.Image = entity.Image;

            _categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}