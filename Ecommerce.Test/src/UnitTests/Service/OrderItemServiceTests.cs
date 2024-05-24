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

    }
}