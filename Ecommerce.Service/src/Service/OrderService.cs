using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class OrderService : BaseService<Order, OrderReadDto, OrderCreateDto, OrderUpdateDto, QueryOptions>, IOrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository,
        IUserRepository userRepository, IProductRepository productRepository,
        IMapper mapper, ICartRepository cartRepository)
            : base(orderRepository, mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<OrderReadDto> CreateOrderFromCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? throw new KeyNotFoundException("Cart not found.");
            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            var order = new Order(userId);

            foreach (var item in cart.CartItems)
            {
                var productItem = await _productRepository.GetByIdAsync(item.ProductId);

                var productSnapshot = new ProductSnapshot
                {
                    ProductId = productItem.Id,
                    Title = productItem.Title,
                    Price = productItem.Price.Value,
                    Description = productItem.Description,
                };

                var orderItem = new OrderItem(order.Id, productSnapshot.ProductId, item.Quantity, productItem.Price.Value);

                orderItem.ProductSnapshot = productSnapshot;

                productItem.Inventory -= item.Quantity;

                await _productRepository.UpdateAsync(productItem);

                order.AddOrUpdateItem(orderItem);
            }
            order.CalculateTotalPrice();

            await _orderRepository.AddAsync(order);

            return _mapper.Map<OrderReadDto>(order);
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var foundOrder = await _orderRepository.GetByIdAsync(orderId);
            DateTime currentDate = DateTime.Now;
            if (foundOrder is null)
            {
                return false;
            }
            else
            {
                TimeSpan timeDifference = currentDate - foundOrder.CreatedAt;
                if (timeDifference <= TimeSpan.FromHours(24))
                {
                    foundOrder.Status = OrderStatus.Cancelled;
                    await _orderRepository.UpdateAsync(foundOrder);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> UpdateOrderItemQuantityAsync(Guid orderId, Guid itemId, int quantity)
        {
            // Retrieve the order by its ID
            var order = await _orderRepository.GetByIdAsync(orderId) ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            // Find the order item by its ID
            var orderItem = order.OrderItems?.FirstOrDefault(item => item.Id == itemId) ?? throw new KeyNotFoundException($"Order item with ID {itemId} not found within order {orderId}.");
            // Update the quantity of the order item
            orderItem.Quantity += quantity;
            // Add or update the modified order item back to the order
            order.AddOrUpdateItem(orderItem);
            // Update the order in the repository
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var foundUser = await _userRepository.GetByIdAsync(userId);
            if (foundUser is not null)
            {
                var result = _orderRepository.GetOrderByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<OrderReadDto>>(result);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
