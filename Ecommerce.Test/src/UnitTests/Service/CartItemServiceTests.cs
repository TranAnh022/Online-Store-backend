using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class CartItemServiceTests
    {
        private readonly CartItemService _service;
        private readonly Mock<IBaseRepository<CartItem, QueryOptions>> _mockCartItemRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;

        public CartItemServiceTests()
        {
            _mockCartItemRepository = new Mock<IBaseRepository<CartItem, QueryOptions>>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CartItemService(_mockCartItemRepository.Object, _mockMapper.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task AddQuantityAsync_ThrowsKeyNotFoundException_WhenProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            _mockProductRepository.Setup(x => x.ExistsAsync(productId)).ReturnsAsync(false);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddQuantityAsync(cartItemId, productId, 5));
        }


        [Fact]
        public async Task AddQuantityAsync_ThrowsKeyNotFoundException_WhenCartItemDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            _mockProductRepository.Setup(x => x.ExistsAsync(productId)).ReturnsAsync(true);
            _mockCartItemRepository.Setup(x => x.GetByIdAsync(cartItemId)).ReturnsAsync((CartItem)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddQuantityAsync(cartItemId, productId, 5));
        }


        [Fact]
        public async Task AddQuantityAsync_ThrowsArgumentException_WhenProductIdsDoNotMatch()
        {
            var productId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();
            var differentProductId = Guid.NewGuid();
            var cartItem = new CartItem { Id = cartItemId, ProductId = differentProductId };

            _mockProductRepository.Setup(x => x.ExistsAsync(productId)).ReturnsAsync(true);
            _mockCartItemRepository.Setup(x => x.GetByIdAsync(cartItemId)).ReturnsAsync(cartItem);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddQuantityAsync(cartItemId, productId, 5));
        }

        [Fact]
        public async Task AddQuantityAsync_Success_WhenValidInput()
        {
            var productId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();
            var quantity = 5;
            var cartItem = new CartItem { Id = cartItemId, ProductId = productId, Quantity = 1 };
            var updatedCartItem = new CartItem { Id = cartItemId, ProductId = productId, Quantity = 6 };
            var cartItemReadDto = new CartItemReadDto { Quantity = 6 };

            _mockProductRepository.Setup(x => x.ExistsAsync(productId)).ReturnsAsync(true);
            _mockCartItemRepository.Setup(x => x.GetByIdAsync(cartItemId)).ReturnsAsync(cartItem);
            _mockCartItemRepository.Setup(x => x.UpdateAsync(cartItem)).ReturnsAsync(updatedCartItem);
            _mockMapper.Setup(m => m.Map<CartItemReadDto>(updatedCartItem)).Returns(cartItemReadDto);

            var result = await _service.AddQuantityAsync(cartItemId, productId, quantity);

            Assert.Equal(6, result.Quantity);
        }
    }
}