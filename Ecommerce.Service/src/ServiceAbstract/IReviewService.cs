using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IReviewService : IBaseService<ReviewReadDto, ReviewCreateDto, ReviewUpdateDto, QueryOptions>
    {
        Task<bool> UpdateRatingAsync(Guid reviewId, int newRating);

        Task<bool> UpdateContextAsync(Guid reviewId, string newContext);

        Task<IEnumerable<ReviewReadDto>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<ReviewReadDto>> GetReviewsByUserIdAsync(Guid userId);


    }
}