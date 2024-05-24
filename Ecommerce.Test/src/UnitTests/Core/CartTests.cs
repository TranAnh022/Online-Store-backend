using FluentAssertions;
using Ecommerce.Core.src.Entities.CartAggregate;
using Xunit;
using Ecommerce.Core.src.Entities;
using System;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class CartTests
    {
        [Fact]
        public void Cart_AddItem_Success()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId);
            var product = new Product("Car", 12, "Car description", Guid.NewGuid(), 20);

            // Act
            cart.AddItem(cartId, product, 2);

            // Assert
            cart.CartItems.Should().HaveCount(1);
        }

        [Fact]
        public void Cart_RemoveItem_Success()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId);
            var productId = Guid.NewGuid();
            var item = new CartItem(productId, cartId, 1);
            var product = new Product("Car", 12, "Car description", productId, 20);
            cart.AddItem(cartId, product, 1);

            // Act
            cart.RemoveItem(product, 1);


            cart.CartItems.Should().BeEmpty();
        }

        [Fact]
        public void Cart_ClearCart_Success()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId);
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var item1 = new CartItem(productId1, cartId, 1);
            var item2 = new CartItem(productId2, cartId, 2);
            cart.AddItem(cartId, new Product("Car1", 12, "Car description", productId1, 20), 1);
            cart.AddItem(cartId, new Product("Car2", 12, "Car description", productId2, 20), 2);

            // Act
            cart.ClearCart();

            // Assert
            cart.CartItems.Should().BeEmpty();
        }
    }
}
