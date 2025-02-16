using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;

namespace Ecommerce.Core.src.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review, QueryOptions>
    {
        // Method to retrieve all reviews for a specific product
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);

    }
}