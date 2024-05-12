using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ICartItemService : IBaseService<CartItemReadDto, CartItemCreateDto, CartItemUpdateDto, QueryOptions>
    {
        Task<CartItemReadDto> AddQuantityAsync(Guid cartItemId, Guid productId, int quantity);
        Task<CartItemReadDto> SetQuantityAsync(Guid cartItemId, Guid productId, int quantity);
    }

}