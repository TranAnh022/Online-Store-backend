using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IOrderService : IBaseService<OrderReadDto, OrderCreateDto, OrderUpdateDto, QueryOptions>
    {
        Task<OrderReadDto> CreateOrderFromCartAsync(Guid userId);
        Task<bool> CancelOrderAsync(Guid orderId);
        Task<bool> UpdateOrderItemQuantityAsync(Guid orderId, Guid itemId, int quantity);

        Task<IEnumerable<OrderReadDto>> GetOrdersByUserIdAsync(Guid userId,string? status);
    }
}