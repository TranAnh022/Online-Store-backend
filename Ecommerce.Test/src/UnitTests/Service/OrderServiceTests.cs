using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockUserRepository.Object,
                _mockProductRepository.Object,
                _mockMapper.Object,
                _mockCartRepository.Object);
        }

        [Fact]
        public async Task CancelOrderAsync_SuccessfullyCancelsOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var order = new Order(userId) { Id = orderId, Status = OrderStatus.Pending };
            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.CancelOrderAsync(orderId);

            // Assert
            Assert.True(result);
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            _mockOrderRepository.Verify(x => x.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task CancelOrderAsync_ThrowsNotFoundException_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.CancelOrderAsync(orderId));
        }

        [Fact]
        public async Task UpdateOrderItemQuantityAsync_WithValidParameters_UpdatesOrderItem()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var initialQuantity = 2;
            var updatedQuantity = 5;
            var order = new Order(orderId);
            var orderItem = new OrderItem(itemId, Guid.NewGuid(), initialQuantity, 100.0m);
            order.AddOrUpdateItem(orderItem);
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.UpdateOrderItemQuantityAsync(orderId, itemId, updatedQuantity);

            // Assert
            Assert.True(result);
            var updatedOrderItem = order.OrderItems.FirstOrDefault(item => item.Id == itemId);
            Assert.NotNull(updatedOrderItem);
            Assert.Equal(updatedQuantity, updatedOrderItem.Quantity);
            _mockOrderRepository.Verify(repo => repo.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderItemQuantityAsync_OrderNotFound_ThrowsException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.UpdateOrderItemQuantityAsync(orderId, itemId, 5));
        }

        [Fact]
        public async Task UpdateOrderItemQuantityAsync_OrderItemNotFound_ThrowsException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var order = new Order(orderId);
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.UpdateOrderItemQuantityAsync(orderId, itemId, 5));
        }
    }
}
