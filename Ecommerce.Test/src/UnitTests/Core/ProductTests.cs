using Ecommerce.Core.src.Entities;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class ProductTests
    {
        [Fact]
        public void UpdateDetails_ShouldUpdateOnlyNonNullValues()
        {
            // Arrange
            var product = new Product("Initial Title", 100m, "Initial Description", Guid.NewGuid(), 10);

            // Act
            product.UpdateDetails("New Title", 150m, null);

            // Assert
            product.Title.Should().Be("New Title");
            product.Price.Should().Be(150m);
            product.Description.Should().Be("Initial Description");
        }

        [Fact]
        public void UpdateCategory_ShouldUpdateCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var product = new Product("Title", 100m, "Description", Guid.NewGuid(), 10);
            var newCategoryId = Guid.NewGuid();

            // Act
            product.UpdateCategory(newCategoryId);

            // Assert
            product.CategoryId.Should().Be(newCategoryId);
        }

        [Fact]
        public void AdjustInventory_ShouldIncreaseInventoryCorrectly()
        {
            // Arrange
            var product = new Product("Title", 100m, "Description", Guid.NewGuid(), 10);

            // Act
            product.AdjustInventory(5);  // Increase inventory

            // Assert
            product.Inventory.Should().Be(15);
        }

        [Fact]
        public void AdjustInventory_WithNegativeValueLeadingToNegativeInventory_ShouldThrowException()
        {
            // Arrange
            var product = new Product("Title", 100m, "Description", Guid.NewGuid(), 5);

            // Act
            Action act = () => product.AdjustInventory(-10);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("Adjustment would result in negative inventory. (Parameter 'Inventory')");
        }

        [Fact]
        public void SetImages_WithNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = new Product("Title", 100m, "Description", Guid.NewGuid(), 10);

            // Act
            Action act = () => product.SetImages(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SetImages_WithEmptyList_ShouldClearImages()
        {
            // Arrange
            var product = new Product("Title", 100m, "Description", Guid.NewGuid(), 10);

            // Act
            product.SetImages(new List<ProductImage>());

            // Assert
            product.Images.Should().BeEmpty();
        }

    }
}