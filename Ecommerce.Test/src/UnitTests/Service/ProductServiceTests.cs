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
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();
        private readonly Mock<IProductImageRepository> _mockProductImageRepository = new Mock<IProductImageRepository>();
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = new Mock<ICategoryRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public ProductServiceTests()
        {
            _productService = new ProductService(_mockProductRepository.Object, _mockMapper.Object, _mockProductImageRepository.Object, _mockCategoryRepository.Object);
        }


        public static IEnumerable<object[]> UpdateProductDetailsData =>
            new List<object[]>
            {
                new object[] { null!, null!, null! },
                new object[] { "Updated Title", null!, null! },
                new object[] { null!, 99.99m, null! },
                new object[] { null!, null!, "Updated Description" }
            };

        [Theory]
        [MemberData(nameof(UpdateProductDetailsData))]
        public async Task UpdateProductDetailsAsync_UpdatesValues_WhenProductExists(string title, decimal? price, string description)
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Title = "Original Title", Price = 50.00M, Description = "Original Description" };
            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductReadDto>(It.IsAny<Product>())).Returns(new ProductReadDto { Title = title ?? product.Title });
            // Act
            var updateDto = new ProductUpdateDto { Title = title, Price = price, Description = description };
            var result = await _productService.UpdateProductDetailsAsync(productId, updateDto);
            // Assert
            Assert.Equal(title ?? "Original Title", result.Title); // Default to original if no update provided
        }

        [Fact]
        public async Task UpdateProductCategoryAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
        {
            var productId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();
            var product = new Product { Id = productId };

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockCategoryRepository.Setup(x => x.GetByIdAsync(newCategoryId)).ReturnsAsync((Category)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateProductCategoryAsync(productId, newCategoryId));
        }

        [Fact]
        public async Task AdjustProductInventoryAsync_AdjustsInventory_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var initialInventory = 10;
            var adjustment = 5;
            var product = new Product { Id = productId, Inventory = initialInventory };

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductReadDto>(It.IsAny<Product>())).Returns(new ProductReadDto { Inventory = initialInventory + adjustment });

            var result = await _productService.AdjustProductInventoryAsync(productId, adjustment);

            Assert.Equal(initialInventory + adjustment, result.Inventory);
        }

        [Fact]
        public async Task AdjustProductInventoryAsync_ThrowsKeyNotFoundException_WhenProductNotFound()
        {
            var productId = Guid.NewGuid();
            var adjustment = 5;

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.AdjustProductInventoryAsync(productId, adjustment));
        }

        [Fact]
        public async Task SetProductImagesAsync_SetsImages_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Images = new List<ProductImage>() };
            var imageDtos = new List<ProductImageCreateDto> { new ProductImageCreateDto { Url = "http://example.com/image1.jpg" } };

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockProductRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductReadDto>(It.IsAny<Product>())).Returns(new ProductReadDto());

            var result = await _productService.SetProductImagesAsync(productId, imageDtos);

            Assert.NotNull(result);
            _mockProductRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetMostPurchased_ReturnsProducts_WhenProductsExist()
        {
            var topNumber = 5;
            var products = new List<Product> { new Product { Id = Guid.NewGuid(), Title = "Bestseller" } };

            _mockProductRepository.Setup(x => x.GetMostPurchasedProductsAsync(topNumber)).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductReadDto>>(products)).Returns(products.Select(p => new ProductReadDto { Title = p.Title }));

            var result = await _productService.GetMostPurchased(topNumber);

            Assert.Single(result);
            Assert.Equal("Bestseller", result.First().Title);
        }

        [Fact]
        public async Task GetMostPurchased_ThrowsInvalidOperationException_WhenNoProductsFound()
        {
            var topNumber = 5;
            _mockProductRepository.Setup(x => x.GetMostPurchasedProductsAsync(topNumber)).ReturnsAsync(new List<Product>());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.GetMostPurchased(topNumber));
        }

        [Theory]
        [InlineData(true, true, null)] // Test case for successful deletion
        [InlineData(false, false, null)] // Test case for failed deletion (user not found)
        [InlineData(false, true, typeof(InvalidOperationException))] // Test case for exception handling
        public async Task DeleteOneAsync_HandlesVariousScenarios_Correctly(bool expectedResult, bool repoSetupResult, Type? exceptionType)
        {
            var productId = Guid.NewGuid();
            // Arrange
            if (exceptionType == null)
            {
                _mockProductRepository.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(repoSetupResult);
            }
            else
            {
                _mockProductRepository.Setup(r => r.DeleteAsync(productId)).ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, "Simulated database error")!);
            }

            // Act & Assert
            if (exceptionType == null)
            {
                var result = await _productService.DeleteOneAsync(productId);
                Assert.Equal(expectedResult, result);
            }
            else
            {
                await Assert.ThrowsAsync(exceptionType, () => _productService.DeleteOneAsync(productId));
            }

            _mockProductRepository.Verify(r => r.DeleteAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "Product 1" },
                new Product { Id = Guid.NewGuid(), Title = "Product 2" },
                new Product { Id = Guid.NewGuid(), Title = "Product 3" },
                new Product { Id = Guid.NewGuid(), Title = "Product 4" },
                new Product { Id = Guid.NewGuid(), Title = "Product 5" }
            };

            _mockProductRepository.Setup(x => x.ListAsync(It.IsAny<ProductQueryOptions>())).ReturnsAsync(products);
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductReadDto>>(products)).Returns(products.Select(p => new ProductReadDto { Title = p.Title }));

            // Act
            var result = await _productService.GetAllAsync(new ProductQueryOptions { Title = "Product", Page = 1, PageSize = 5, SortBy = "Name", SortOrder = "asc" });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetOneByIdAsync_ReturnsProduct_WhenValidIdProvided()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Title = "Test Product" };

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductReadDto>(product)).Returns(new ProductReadDto { Title = product.Title });

            // Act
            var result = await _productService.GetOneByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Title);
        }
    }
}
