using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class CartService : BaseService<Cart, CartReadDto, CartCreateDto, CartUpdateDto, QueryOptions>, ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IMapper mapper, IUserRepository userRepository, IProductRepository productRepository)
            : base(cartRepository, mapper)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public override async Task<IEnumerable<CartReadDto>> GetAllAsync(QueryOptions options)
        {
            var carts = await _cartRepository.ListAsync(options);
            return _mapper.Map<IEnumerable<CartReadDto>>(carts);
        }

        public async Task<CartItem> AddItemToCartAsync(Guid cartId, CartItemCreateDto itemDto)
        {
            // Retrieve the cart to ensure it exists
            var cart = await _cartRepository.GetByIdAsync(cartId) ?? throw new KeyNotFoundException("Cart not found.");

            // Map the DTO to a new CartItem entity
            var item = _mapper.Map<CartItem>(itemDto);
            item.CartId = cartId; // Set the CartId explicitly
            // item.Id = Guid.NewGuid(); // Generate a new ID

            // Retrieve the product to adjust inventory
            var product = await _productRepository.GetByIdAsync(item.ProductId) ?? throw new KeyNotFoundException("Product not found.");

            // Adjust product inventory
            product.AdjustInventory(-item.Quantity);

            // Add item to the cart
            cart.AddItem(item);

            // Update the product in the database to reflect inventory change
            await _productRepository.UpdateAsync(product);

            // Update the cart in the database to include the new item
            await _cartRepository.UpdateAsync(cart);

            return item;
        }

        public async Task<bool> ClearCartAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId) ?? throw new KeyNotFoundException("Cart not found.");
            cart.ClearCart();
            await _cartRepository.UpdateAsync(cart);
            return true;
        }

        public async Task<CartReadDto> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? throw new KeyNotFoundException("Cart not found.");
            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid itemId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId) ?? throw new KeyNotFoundException("Cart not found.");
            if (cart.CartItems == null || cart.CartItems.Count == 0)
            {
                throw new InvalidOperationException("There are no items in the cart.");
            }
            var item = cart.CartItems.FirstOrDefault(i => i.Id == itemId) ?? throw new KeyNotFoundException("Item not found in cart.");
            if (cart.RemoveItem(item))
            {
                await _cartRepository.UpdateAsync(cart);
                return true;
            }
            return false;
        }
    }
}