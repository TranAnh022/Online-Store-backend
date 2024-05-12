using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;

namespace Ecommerce.Core.src.Interfaces
{
    public interface ICartRepository : IBaseRepository<Cart, QueryOptions>
    {
        // Update a cart item within a specific cart
        Task<CartItem> UpdateCartItemAsync(Guid cartId, CartItem cartItem);

        // Remove a specific item from the cart
        Task<bool> DeleteCartItemAsync(Guid cartItemId);

        // Method to retrieve a cart by its associated user ID
        Task<Cart> GetCartByUserIdAsync(Guid userId);

        // Method to create cart when user created 
        Task<Cart> CreateCartForUser(Guid userId);
        // Method to ensure that all users have a cart
        Task EnsureCartsForAllUsers();
    }
}