using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_ShouldAddItem_WhenNewItemProvided()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var productSnapshot = new ProductSnapshot(Guid.NewGuid(), "Laptop", 999.99m, "Latest model");
            var orderItem = new OrderItem(Guid.NewGuid(), productSnapshot.ProductId, 1, 999.99m);

            // Act
            Action act = () => order.AddOrUpdateItem(orderItem);

            // Assert
            act.Should().NotThrow();
            order.OrderItems.Should().ContainSingle();
        }

        [Fact]
        public void AddOrUpdateItem_ShouldUpdateQuantity_WhenExistingItemAdded()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var productSnapshot = new ProductSnapshot(Guid.NewGuid(), "Laptop", 999.99m, "Latest model");
            var orderItem = new OrderItem(Guid.NewGuid(), productSnapshot.ProductId, 1, 999.99m);
            order.AddOrUpdateItem(orderItem);

            // Act
            var newItem = new OrderItem(orderItem.Id, orderItem.ProductSnapshotId, 2, 999.99m);
            order.AddOrUpdateItem(newItem);

            // Assert
            var updatedItem = order.OrderItems.FirstOrDefault(i => i.Id == newItem.Id);
            Assert.Contains(orderItem, order.OrderItems);
            Assert.NotNull(updatedItem);
            Assert.Equal(2, updatedItem.Quantity);
        }

        [Fact]
        public void RemoveItem_ShouldDecreaseTotalPrice_WhenItemRemoved()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var productSnapshot = new ProductSnapshot(Guid.NewGuid(), "Laptop", 999.99m, "Latest model");
            var orderItem = new OrderItem(Guid.NewGuid(), productSnapshot.ProductId, 2, 999.99m);
            order.AddOrUpdateItem(orderItem);

            // Act
            bool result = order.RemoveItem(orderItem.Id);

            // Assert
            result.Should().BeTrue();
            order.OrderItems.Should().BeEmpty();
        }

        [Fact]
        public void RemoveItem_ShouldReturnFalse_WhenItemNotFound()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            var itemId = Guid.NewGuid();

            // Act
            bool result = order.RemoveItem(itemId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void UpdateStatus_ShouldChangeStatus_WhenNewValidStatusProvided()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());

            // Act
            order.UpdateStatus(OrderStatus.Shipped);

            // Assert
            order.Status.Should().Be(OrderStatus.Shipped);
        }

        [Fact]
        public void UpdateStatus_ShouldThrowArgumentOutOfRangeException_WhenInvalidStatusProvided()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());

            // Act
            Action act = () => order.UpdateStatus((OrderStatus)123);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}