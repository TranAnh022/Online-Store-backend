using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.Service
{
    public class CartItemService : BaseService<CartItem, CartItemReadDto, CartItemCreateDto, CartItemUpdateDto, QueryOptions>, ICartItemService
    {
        private readonly IProductRepository _productRepository;
        public CartItemService(IBaseRepository<CartItem, QueryOptions> repository, IMapper mapper, IProductRepository productRepository)
        : base(repository, mapper)
        {
            _productRepository = productRepository;
        }
        public async Task<CartItemReadDto> AddQuantityAsync(Guid cartItemId, Guid productId, int quantity)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw  CustomExeption.NotFoundException($"No product found with ID {productId}");
            }
            var cartItem = await _repository.GetByIdAsync(cartItemId) ?? throw CustomExeption.NotFoundException($"CartItem with ID {cartItemId} not found");
            if (cartItem.ProductId != productId)
            {
                throw CustomExeption.NotFoundException("The product ID does not match the cart item's product.");
            }
            cartItem.AddQuantity(quantity);
            var updatedCartItem = await _repository.UpdateAsync(cartItem);
            return _mapper.Map<CartItemReadDto>(updatedCartItem);
        }

        public async Task<CartItemReadDto> SetQuantityAsync(Guid cartItemId, Guid productId, int quantity)
        {
            if (!await _productRepository.ExistsAsync(productId))
            {
                throw CustomExeption.NotFoundException($"No product found with ID {productId}");
            }
            var cartItem = await _repository.GetByIdAsync(cartItemId) ?? throw new KeyNotFoundException($"CartItem with ID {cartItemId} not found");
            if (cartItem.ProductId != productId)
            {
                throw CustomExeption.NotFoundException("The product ID does not match the cart item's product.");
            }
            cartItem.SetQuantity(quantity);
            var updatedCartItem = await _repository.UpdateAsync(cartItem);
            return _mapper.Map<CartItemReadDto>(updatedCartItem);
        }


    }
}
