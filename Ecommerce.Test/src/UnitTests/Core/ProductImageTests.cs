using Ecommerce.Core.src.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests
{
    public class ProductImageTests
    {
        [Fact]
        public void UpdateUrl_WithValidUrl_ShouldUpdateUrl()
        {
            // Arrange
            var productImage = new ProductImage(Guid.NewGuid(), "https://example.com/oldimage.jpg");

            // Act
            productImage.UpdateUrl("https://example.com/newimage.jpg");

            // Assert
            productImage.Url.Should().Be("https://example.com/newimage.jpg");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UpdateUrl_WithNullOrEmptyUrl_ShouldThrowArgumentException(string? invalidUrl)
        {
            // Arrange
            var productImage = new ProductImage(Guid.NewGuid(), "https://example.com/image.jpg");

            // Act
            var action = () => productImage.UpdateUrl(invalidUrl!);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("URL cannot be null or empty. (Parameter 'newUrl')");
        }

        [Fact]
        public void UpdateUrl_WithInvalidUrl_ShouldThrowArgumentException()
        {
            // Arrange
            var productImage = new ProductImage(Guid.NewGuid(), "https://example.com/image.jpg");

            // Act
            var action = () => productImage.UpdateUrl("ht://invalid-url");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("URL must be a valid, well-formed URL and start with 'http://' or 'https://'. (Parameter 'newUrl')");
        }
    }
}