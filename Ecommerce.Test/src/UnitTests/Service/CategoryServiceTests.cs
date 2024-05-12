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
    public class CategoryServiceTests
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository = new Mock<ICategoryRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public CategoryServiceTests()
        {
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task UpdateCategoryNameAsync_UpdatesCategoryName_WhenValidNameProvided()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var newName = "New Category Name";
            var category = new Category("Old Category Name", "http://example.com/old_image.jpg");

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(It.IsAny<Category>())).Returns(new CategoryReadDto { Name = newName });

            // Act
            var result = await _categoryService.UpdateCategoryNameAsync(categoryId, newName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newName, result.Name);
        }

        [Fact]
        public async Task UpdateCategoryImageAsync_UpdatesCategoryImage_WhenValidUrlProvided()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var newImageUrl = "http://example.com/new_image.jpg";
            var category = new Category("Category Name", "http://example.com/old_image.jpg");

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(It.IsAny<Category>())).Returns(new CategoryReadDto { Image = newImageUrl });

            // Act
            var result = await _categoryService.UpdateCategoryImageAsync(categoryId, newImageUrl);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newImageUrl, result.Image);
        }

        [Fact]
        public async Task UpdateCategoryNameAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var newName = "New Category Name";

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync((Category)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.UpdateCategoryNameAsync(categoryId, newName));
        }

        [Fact]
        public async Task UpdateCategoryImageAsync_ThrowsKeyNotFoundException_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var newImageUrl = "http://example.com/new_image.jpg";

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync((Category)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.UpdateCategoryImageAsync(categoryId, newImageUrl));
        }

        [Fact]
        public async Task CreateOneAsync_CreatesCategory_WhenValidDtoProvided()
        {
            // Arrange
            var createDto = new CategoryCreateDto { Name = "New Category", Image = "http://example.com/image.jpg" };
            var expectedCategory = new Category("New Category", "http://example.com/image.jpg");

            _mockMapper.Setup(m => m.Map<Category>(createDto)).Returns(expectedCategory);
            _mockCategoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(expectedCategory);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(expectedCategory)).Returns(new CategoryReadDto { Name = "New Category", Image = "http://example.com/image.jpg" });

            // Act
            var result = await _categoryService.CreateOneAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Category", result.Name);
            Assert.Equal("http://example.com/image.jpg", result.Image);
        }

        [Theory]
        [InlineData(true, true, null)] // Test case for successful deletion
        [InlineData(false, false, null)] // Test case for failed deletion (user not found)
        [InlineData(false, true, typeof(InvalidOperationException))] // Test case for exception handling
        public async Task DeleteOneAsync_HandlesVariousScenarios_Correctly(bool expectedResult, bool repoSetupResult, Type? exceptionType)
        {
            var categoryId = Guid.NewGuid();
            // Arrange
            if (exceptionType == null)
            {
                _mockCategoryRepository.Setup(r => r.DeleteAsync(categoryId)).ReturnsAsync(repoSetupResult);
            }
            else
            {
                _mockCategoryRepository.Setup(r => r.DeleteAsync(categoryId)).ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, "Simulated database error")!);
            }

            // Act & Assert
            if (exceptionType == null)
            {
                var result = await _categoryService.DeleteOneAsync(categoryId);
                Assert.Equal(expectedResult, result);
            }
            else
            {
                await Assert.ThrowsAsync(exceptionType, () => _categoryService.DeleteOneAsync(categoryId));
            }

            _mockCategoryRepository.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories_WhenCalled()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category("Category 1", "http://example.com/image1.jpg"),
                new Category("Category 2", "http://example.com/image2.jpg"),
                new Category("Category 3", "http://example.com/image3.jpg")
            };

            var queryOptions = new QueryOptions { Page = 1, PageSize = 3, SortBy = "Name", SortOrder = "asc" };

            _mockCategoryRepository.Setup(x => x.ListAsync(queryOptions)).ReturnsAsync(categories);
            _mockMapper.Setup(m => m.Map<IEnumerable<CategoryReadDto>>(categories))
                .Returns(categories.Select(c => new CategoryReadDto { Name = c.Name, Image = c.Image }));

            // Act
            var result = await _categoryService.GetAllAsync(queryOptions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetOneByIdAsync_ReturnsCategory_WhenValidIdProvided()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category("Category", "http://example.com/image.jpg");

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(category))
                .Returns(new CategoryReadDto { Name = category.Name, Image = category.Image });

            // Act
            var result = await _categoryService.GetOneByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Category", result.Name);
            Assert.Equal("http://example.com/image.jpg", result.Image);
        }

        [Fact]
        public async Task UpdateOneAsync_UpdatesCategory_WhenValidDtoProvided()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateDto = new CategoryUpdateDto { Name = "Updated Category", Image = "http://example.com/updated_image.jpg" };
            var category = new Category("Category", "http://example.com/image.jpg");

            _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(It.IsAny<Category>()))
                .Returns(new CategoryReadDto { Name = "Updated Category", Image = "http://example.com/updated_image.jpg" });

            // Act
            var result = await _categoryService.UpdateOneAsync(categoryId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Category", result.Name);
            Assert.Equal("http://example.com/updated_image.jpg", result.Image);
        }
    }

}
