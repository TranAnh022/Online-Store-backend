using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class ReviewService : BaseService<Review, ReviewReadDto, ReviewCreateDto, ReviewUpdateDto, QueryOptions>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IProductRepository productRepository, IUserRepository userRepository)
           : base(reviewRepository, mapper)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<ReviewReadDto> CreateOneAsync(Guid userId, ReviewCreateDto createDto)
        {
            // Validate existence of user and product using their IDs
            var userExists = await _userRepository.ExistsAsync(userId);
            var productExists = await _productRepository.ExistsAsync(createDto.ProductId);

            if (!userExists || !productExists)
            {
                throw new KeyNotFoundException("User or product not found");
            }

            // Map DTO to Review entity, and set user and product IDs
            var newReview = _mapper.Map<Review>(createDto);
            newReview.UserId = userId;  // Set user ID
            newReview.ProductId = createDto.ProductId;  // Set product ID

            // Add new review to the repository and save changes
            var result = await _reviewRepository.AddAsync(newReview);
            return _mapper.Map<ReviewReadDto>(result);
        }
    
        public async Task<IEnumerable<ReviewReadDto>> GetReviewsByProductIdAsync(Guid productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }

    }
}
