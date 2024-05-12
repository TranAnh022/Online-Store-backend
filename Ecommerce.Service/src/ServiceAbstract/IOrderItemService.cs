using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IOrderItemService : IBaseService<OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto, QueryOptions>
    {
      
    }
}