using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Review> _reviews;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
            _reviews = context.Reviews;
        }
        public async Task<Review> AddAsync(Review entity)
        {
            await _reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var review = await _reviews.FindAsync(id);
            if (review == null)
                return false;
            _reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Review> GetByIdAsync(Guid id)
        {
            return await _reviews.FindAsync(id) ?? throw new InvalidOperationException("Review not found");
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId)
        {
            return await _reviews
                                 .Include(r => r.User)
                                 .Include(r => r.Product)
                                 .Where(r => r.ProductId == productId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _reviews
                                 .Include(r => r.User)
                                 .Include(r => r.Product)
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Review>> ListAsync(QueryOptions options)
        {
            var query = _reviews.AsQueryable();

            if (!string.IsNullOrEmpty(options.Search))
            {
                query = query.Where(r => r.Context!.Contains(options.Search));
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            if (!string.IsNullOrEmpty(options.SortBy))
            {
                query = options.SortBy.ToLower() switch
                {
                    "rating" => options.SortOrder == "asc" ? query.OrderBy(r => r.Rating) : query.OrderByDescending(r => r.Rating),
                    _ => query
                };
            }

            return await query.ToListAsync();
        }

        public async Task<Review> UpdateAsync(Review entity)
        {
            var existingReview = await _reviews.FindAsync(entity.Id);
            if (existingReview == null)
            {
                return null!;
            }

            existingReview.Rating = entity.Rating;
            existingReview.Context = entity.Context;

            _context.Update(existingReview);
            await _context.SaveChangesAsync();
            return existingReview;
        }
    }
}