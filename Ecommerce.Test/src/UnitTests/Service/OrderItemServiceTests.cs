using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IBaseRepository<OrderItem, QueryOptions>> _mockOrderItemRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderItemService _orderItemService;

        public OrderItemServiceTests()
        {
            _mockOrderItemRepository = new Mock<IBaseRepository<OrderItem, QueryOptions>>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _orderItemService = new OrderItemService(_mockOrderItemRepository.Object, _mockOrderRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetOrderItemDetailsAsync_WithValidId_ReturnsOrderItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var orderItem = new OrderItem { Id = itemId };
            _mockOrderItemRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(orderItem);
            _mockMapper.Setup(mapper => mapper.Map<OrderItemReadDto>(orderItem)).Returns(new OrderItemReadDto());

            // Act
            var result = await _orderItemService.GetOrderItemDetailsAsync(itemId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetOrderItemsByOrderIdAsync_WithValidOrderId_ReturnsOrderItems()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var orderItems = new List<OrderItem>
             {
                new OrderItem { Id = Guid.NewGuid(), OrderId = orderId },
                new OrderItem { Id = Guid.NewGuid(), OrderId = orderId }
             };
            var order = new Order(orderId);
            foreach (var item in orderItems)
            {
                order.AddOrUpdateItem(item);
            }

            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderItemReadDto>>(It.IsAny<IEnumerable<OrderItem>>())).Returns(new List<OrderItemReadDto>());

            // Act
            var result = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
        }

    }
}