using Ecommerce.Core.src.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests
{
    public class CategoryTests
    {
        [Fact]
        public void UpdateName_WithValidName_ShouldUpdateName()
        {
            // Arrange
            var category = new Category("Initial Name", "https://example.com/image.jpg");

            // Act
            category.UpdateName("Updated Name");

            // Assert
            category.Name.Should().Be("Updated Name");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateName_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
        {
            // Arrange
            var category = new Category("Initial Name", "https://example.com/image.jpg");

            // Act
            Action act = () => category.UpdateName(invalidName!);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Category name cannot be null or empty. (Parameter 'name')");
        }

        [Fact]
        public void UpdateImage_WithValidUrl_ShouldUpdateImageUrl()
        {
            // Arrange
            var category = new Category("Category", "https://example.com/oldimage.jpg");

            // Act
            category.UpdateImage("https://example.com/newimage.jpg");

            // Assert
            category.Image.Should().Be("https://example.com/newimage.jpg");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateImage_WithNullOrEmptyUrl_ShouldThrowArgumentException(string? invalidUrl)
        {
            // Arrange
            var category = new Category("Category", "https://example.com/image.jpg");

            // Act
            Action act = () => category.UpdateImage(invalidUrl!);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Image URL cannot be null or empty. (Parameter 'image')");
        }

        [Fact]
        public void UpdateImage_WithInvalidUrlFormat_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category("Category", "https://example.com/image.jpg");

            // Act
            Action act = () => category.UpdateImage("invalidurl");

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Image URL must be a valid URL. (Parameter 'image')");
        }
    }
}