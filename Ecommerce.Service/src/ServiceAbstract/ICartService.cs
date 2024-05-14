using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ICartService : IBaseService<CartReadDto, CartCreateDto, CartUpdateDto, QueryOptions>
    {
        Task<CartReadDto> GetCartByUserIdAsync(Guid userId);

        Task<CartReadDto> AddItemToCartAsync(Guid productId,int quantity,Guid userId);

        Task<bool> RemoveItemFromCartAsync(Guid productId, int quantity, Guid userId);



    }
}