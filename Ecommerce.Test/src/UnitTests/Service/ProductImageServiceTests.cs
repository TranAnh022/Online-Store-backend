using AutoMapper;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class ProductImageServiceTests
    {
        private readonly ProductImageService _productImageService;
        private readonly Mock<IProductImageRepository> _mockProductImageRepository = new Mock<IProductImageRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public ProductImageServiceTests()
        {
            _productImageService = new ProductImageService(_mockProductImageRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task UpdateImageUrlAsync_UpdatesUrl_WhenValidImageIdAndUrlProvided()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var newUrl = "http://example.com/new-image.jpg";
            var productImage = new ProductImage(imageId, "http://example.com/old-image.jpg");

            _mockProductImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync(productImage);
            _mockProductImageRepository.Setup(x => x.UpdateAsync(It.IsAny<ProductImage>())).ReturnsAsync(productImage);
            _mockMapper.Setup(m => m.Map<ProductImageReadDto>(It.IsAny<ProductImage>())).Returns(new ProductImageReadDto { Url = newUrl });

            // Act
            var result = await _productImageService.UpdateImageUrlAsync(imageId, newUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUrl, result.Url);
        }

        [Fact]
        public async Task UpdateImageUrlAsync_ThrowsKeyNotFoundException_WhenImageNotFound()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var newUrl = "http://example.com/new-image.jpg";

            _mockProductImageRepository.Setup(x => x.GetByIdAsync(imageId)).ReturnsAsync((ProductImage)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productImageService.UpdateImageUrlAsync(imageId, newUrl));
        }
    }
}
