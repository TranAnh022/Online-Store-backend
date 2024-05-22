using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IReviewService : IBaseService<ReviewReadDto, ReviewCreateDto, ReviewUpdateDto, QueryOptions>
    {
        Task<IEnumerable<ReviewReadDto>> GetReviewsByProductIdAsync(Guid productId);

        Task<ReviewReadDto> CreateOneAsync(ReviewCreateDto reviewCreateDto, Guid userId);
    }
}