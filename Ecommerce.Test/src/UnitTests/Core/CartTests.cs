using FluentAssertions;
using Ecommerce.Core.src.Entities.CartAggregate;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class CartTests
    {
        [Fact]
        public void Cart_AddItem_Success()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            cart.AddItem(item);

            // Assert
            Assert.Single(cart.CartItems!);
        }

        [Fact]
        public void Cart_AddItem_NullItem_ThrowsException()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cart.AddItem(null!));
        }

        [Fact]
        public void Cart_RemoveItem_Success()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);
            cart.AddItem(item);

            // Act
            var result = cart.RemoveItem(item);

            // Assert
            Assert.True(result);
            Assert.Empty(cart.CartItems!);
        }

        [Fact]
        public void Cart_RemoveItem_NonExistingItem_ReturnsFalse()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var item = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            var result = cart.RemoveItem(item);

            // Assert
            Assert.False(result);
            Assert.Empty(cart.CartItems!);
        }

        [Fact]
        public void Cart_ClearCart_Success()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid());
            var item1 = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1);
            var item2 = new CartItem(Guid.NewGuid(), Guid.NewGuid(), 2);
            cart.AddItem(item1);
            cart.AddItem(item2);

            // Act
            cart.ClearCart();

            // Assert
            Assert.Empty(cart.CartItems!);
        }
    }
}