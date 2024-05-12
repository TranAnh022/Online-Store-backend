using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class OrderItemService : BaseService<OrderItem, OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto, QueryOptions>, IOrderItemService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderItemService(IBaseRepository<OrderItem, QueryOptions> repository, IOrderRepository orderRepository, IMapper mapper)
            : base(repository, mapper)
        {
            _orderRepository = orderRepository;
        }
        public override async Task<IEnumerable<OrderItemReadDto>> GetAllAsync(QueryOptions queryOptions)
        {
            var orderItems= await _orderRepository.ListAsync(queryOptions);
            return _mapper.Map<IEnumerable<OrderItemReadDto>>(orderItems);
        }
    }
}