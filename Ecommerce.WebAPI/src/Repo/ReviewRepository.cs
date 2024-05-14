using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ReviewRepository :BaseRepo<Review,QueryOptions>,IReviewRepository
    {

        public ReviewRepository(AppDbContext context):base(context)
        {
        }
        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId)
        {
            return await _data
                                 .Include(r => r.User)
                                 .Include(r => r.Product)
                                 .Where(r => r.ProductId == productId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Review>> ListAsync(QueryOptions options)
        {
            var query = _data.AsQueryable();

            if (!string.IsNullOrEmpty(options.Search))
            {
                query = query.Where(r => r.Context!.Contains(options.Search));
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            return await query.ToListAsync();
        }
    }
}