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

        public CartService(ICartRepository cartRepository, IMapper mapper)
            : base(cartRepository, mapper)
        {
            _cartRepository = cartRepository;
        }

        public override async Task<IEnumerable<CartReadDto>> GetAllAsync(QueryOptions options)
        {
            var carts = await _cartRepository.ListAsync(options);
            return _mapper.Map<IEnumerable<CartReadDto>>(carts);
        }

        public async Task<CartReadDto> AddItemToCartAsync(Guid productId, int quantity, Guid userId)
        {

            var cart = await _cartRepository.AddCartItemToCart(productId, quantity, userId);
            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartReadDto> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? throw new KeyNotFoundException("Cart not found.");
            return _mapper.Map<CartReadDto>(cart);
        }

        public async Task<CartReadDto> RemoveItemFromCartAsync(Guid productId, int quantity, Guid userId)
        {
            var cart = await _cartRepository.RemoveCartItem(productId, quantity, userId);
            return _mapper.Map<CartReadDto>(cart);
        }
    }
}