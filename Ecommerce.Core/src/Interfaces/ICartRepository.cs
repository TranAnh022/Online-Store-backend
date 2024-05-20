using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;

namespace Ecommerce.Core.src.Interfaces
{
    public interface ICartRepository : IBaseRepository<Cart, QueryOptions>
    {
        // Update a cart item within a specific cart
        Task<Cart> AddCartItemToCart(Guid productId,int quantity,Guid userId);

        // Remove a specific item from the cart
        Task<Cart> RemoveCartItem(Guid productId, int quantity, Guid userId);

        Task<Cart> GetCartByUserIdAsync(Guid userId);
    }
}