using FluentAssertions;
using Ecommerce.Core.src.Entities.CartAggregate;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class CartItemTests
    {
        [Fact]
        public void CartItem_AddQuantity_Success()
        {
            // Arrange
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            item.AddQuantity(5);

            // Assert
            Assert.Equal(6, item.Quantity);
        }

        [Fact]
        public void CartItem_AddQuantity_InvalidQuantity_ThrowsException()
        {
            // Arrange
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => item.AddQuantity(-5));
        }

        [Fact]
        public void CartItem_SetQuantity_Success()
        {
            // Arrange
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            item.SetQuantity(10);

            // Assert
            Assert.Equal(10, item.Quantity);
        }

        [Fact]
        public void CartItem_SetQuantity_InvalidQuantity_ThrowsException()
        {
            // Arrange
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => item.SetQuantity(-5));
        }
    }
}