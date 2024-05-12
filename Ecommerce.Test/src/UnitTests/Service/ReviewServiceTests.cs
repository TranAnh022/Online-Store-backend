using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class ReviewServiceTests
    {
        private readonly ReviewService _reviewService;
        private readonly Mock<IReviewRepository> _mockReviewRepository = new Mock<IReviewRepository>();
        private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public ReviewServiceTests()
        {
            _reviewService = new ReviewService(_mockReviewRepository.Object, _mockMapper.Object, _mockProductRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateOneAsync_ThrowsKeyNotFoundException_WhenUserOrProductNotFound()
        {
            var userId = Guid.NewGuid();
            var createDto = new ReviewCreateDto { ProductId = Guid.NewGuid() };
            _mockUserRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(false);
            _mockProductRepository.Setup(x => x.ExistsAsync(createDto.ProductId)).ReturnsAsync(true);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.CreateOneAsync(userId, createDto));
        }

        [Fact]
        public async Task CreateOneAsync_CreatesReviewSuccessfully_WhenUserAndProductExist()
        {
            var userId = Guid.NewGuid();
            var createDto = new ReviewCreateDto { ProductId = Guid.NewGuid() };
            var review = new Review { UserId = userId, ProductId = createDto.ProductId };
            _mockUserRepository.Setup(x => x.ExistsAsync(userId)).ReturnsAsync(true);
            _mockProductRepository.Setup(x => x.ExistsAsync(createDto.ProductId)).ReturnsAsync(true);
            _mockMapper.Setup(x => x.Map<Review>(createDto)).Returns(review);
            _mockReviewRepository.Setup(x => x.AddAsync(review)).ReturnsAsync(review);
            _mockMapper.Setup(x => x.Map<ReviewReadDto>(review)).Returns(new ReviewReadDto());

            var result = await _reviewService.CreateOneAsync(userId, createDto);

            Assert.NotNull(result);
            _mockReviewRepository.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task UpdateRatingAsync_UpdatesRatingSuccessfully_WhenReviewExists(int newRating)
        {
            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId, Rating = 3 };
            _mockReviewRepository.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(review);
            _mockReviewRepository.Setup(x => x.UpdateAsync(review)).ReturnsAsync(review);

            var result = await _reviewService.UpdateRatingAsync(reviewId, newRating);

            Assert.True(result);
            Assert.Equal(newRating, review.Rating);
            _mockReviewRepository.Verify(x => x.UpdateAsync(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRatingAsync_ThrowsKeyNotFoundException_WhenReviewNotFound()
        {
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync((Review)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.UpdateRatingAsync(reviewId, 4));
        }

        [Fact]
        public async Task UpdateContextAsync_UpdatesContext_WhenReviewExists()
        {
            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId, Context = "Old context" };
            var newContext = "New updated context";

            _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync(review);
            _mockReviewRepository.Setup(r => r.UpdateAsync(review)).ReturnsAsync(review);

            var result = await _reviewService.UpdateContextAsync(reviewId, newContext);

            Assert.True(result);
            Assert.Equal(newContext, review.Context);
            _mockReviewRepository.Verify(r => r.UpdateAsync(review), Times.Once);
        }

        [Fact]
        public async Task UpdateContextAsync_ThrowsKeyNotFoundException_WhenReviewNotFound()
        {
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync((Review)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.UpdateContextAsync(reviewId, "Some context"));
        }

        [Fact]
        public async Task DeleteOneAsync_ReturnsTrue_WhenReviewDeleted()
        {
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.DeleteAsync(reviewId)).ReturnsAsync(true);

            var result = await _reviewService.DeleteOneAsync(reviewId);

            Assert.True(result);
            _mockReviewRepository.Verify(r => r.DeleteAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task DeleteOneAsync_ReturnsFalse_WhenDeleteFails()
        {
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.DeleteAsync(reviewId)).ReturnsAsync(false);

            var result = await _reviewService.DeleteOneAsync(reviewId);

            Assert.False(result);
            _mockReviewRepository.Verify(r => r.DeleteAsync(reviewId), Times.Once);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsAllReviews()
        {
            var reviews = new List<Review> { new Review { Id = Guid.NewGuid() } };
            _mockReviewRepository.Setup(r => r.ListAsync(It.IsAny<QueryOptions>())).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(reviews)).Returns(reviews.Select(r => new ReviewReadDto()));

            var results = await _reviewService.GetAllAsync(new QueryOptions());

            Assert.NotEmpty(results);
            _mockReviewRepository.Verify(r => r.ListAsync(It.IsAny<QueryOptions>()), Times.Once);
        }

        [Fact]
        public async Task GetOneByIdAsync_ReturnsReview_WhenFound()
        {
            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId };
            _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync(review);
            _mockMapper.Setup(m => m.Map<ReviewReadDto>(review)).Returns(new ReviewReadDto());

            var result = await _reviewService.GetOneByIdAsync(reviewId);

            Assert.NotNull(result);
            _mockReviewRepository.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task GetOneByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
        {
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync((Review)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.GetOneByIdAsync(reviewId));
        }

        [Fact]
        public async Task UpdateOneAsync_UpdatesReview_WhenExists()
        {
            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId };
            var updateDto = new ReviewUpdateDto();

            _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId)).ReturnsAsync(review);
            _mockReviewRepository.Setup(r => r.UpdateAsync(review)).ReturnsAsync(review);
            _mockMapper.Setup(m => m.Map(updateDto, review)).Returns(review);
            _mockMapper.Setup(m => m.Map<ReviewReadDto>(review)).Returns(new ReviewReadDto());

            var result = await _reviewService.UpdateOneAsync(reviewId, updateDto);

            Assert.NotNull(result);
            _mockReviewRepository.Verify(r => r.GetByIdAsync(reviewId), Times.Once);
            _mockReviewRepository.Verify(r => r.UpdateAsync(review), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByProductIdAsync_ReturnsReviews_WhenReviewsExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), ProductId = productId, Rating = 5, Context = "Great!" },
                new Review { Id = Guid.NewGuid(), ProductId = productId, Rating = 4, Context = "Good!" }
            };

            var reviewDtos = new List<ReviewReadDto>
            {
                new ReviewReadDto { Id = reviews[0].Id, ProductId = productId, Rating = 5, Context = "Great!" },
                new ReviewReadDto { Id = reviews[1].Id, ProductId = productId, Rating = 4, Context = "Good!" }
            };

            _mockReviewRepository.Setup(repo => repo.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(It.IsAny<IEnumerable<Review>>()))
                .Returns(reviewDtos);

            // Act
            var result = await _reviewService.GetReviewsByProductIdAsync(productId);

            // Assert
            Assert.Equal(2, result.Count());
            _mockReviewRepository.Verify(x => x.GetReviewsByProductIdAsync(productId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ReviewReadDto>>(reviews), Times.Once);

        }

        [Fact]
        public async Task GetReviewsByProductIdAsync_ReturnsEmpty_WhenNoReviewsExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var emptyReviews = new List<Review>();
            var emptyReviewDtos = new List<ReviewReadDto>();

            _mockReviewRepository.Setup(x => x.GetReviewsByProductIdAsync(productId))
                .ReturnsAsync(emptyReviews);

            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(It.IsAny<IEnumerable<Review>>()))
                .Returns(emptyReviewDtos);

            // Act
            var result = await _reviewService.GetReviewsByProductIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockReviewRepository.Verify(x => x.GetReviewsByProductIdAsync(productId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ReviewReadDto>>(emptyReviews), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByUserIdAsync_ReturnsReviews_WhenReviewsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), UserId = userId, Rating = 5, Context = "Excellent!" },
                new Review { Id = Guid.NewGuid(), UserId = userId, Rating = 4, Context = "Pretty good!" }
            };

            var reviewDtos = new List<ReviewReadDto>
            {
                new ReviewReadDto { Id = reviews[0].Id, UserId = userId, Rating = 5, Context = "Excellent!" },
                new ReviewReadDto { Id = reviews[1].Id, UserId = userId, Rating = 4, Context = "Pretty good!" }
            };

            _mockReviewRepository.Setup(x => x.GetReviewsByUserIdAsync(userId))
                .ReturnsAsync(reviews);

            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(It.IsAny<IEnumerable<Review>>()))
                .Returns(reviewDtos);

            // Act
            var result = await _reviewService.GetReviewsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockReviewRepository.Verify(x => x.GetReviewsByUserIdAsync(userId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ReviewReadDto>>(reviews), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByUserIdAsync_ReturnsEmpty_WhenNoReviewsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockReviewRepository.Setup(x => x.GetReviewsByUserIdAsync(userId))
                .ReturnsAsync(new List<Review>());
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(It.IsAny<IEnumerable<Review>>()))
                .Returns(new List<ReviewReadDto>());

            // Act
            var result = await _reviewService.GetReviewsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockReviewRepository.Verify(x => x.GetReviewsByUserIdAsync(userId), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ReviewReadDto>>(It.IsAny<IEnumerable<Review>>()), Times.Once);
        }
    }
}