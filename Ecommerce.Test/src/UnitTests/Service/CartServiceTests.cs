using AutoMapper;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class CartServiceTests
    {
        private readonly CartService _cartService;
        private readonly Mock<ICartRepository> _mockCartRepository = new Mock<ICartRepository>();
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IProductRepository> _mockProductRepository = new Mock<IProductRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public CartServiceTests()
        {
            _cartService = new CartService(_mockCartRepository.Object, _mockMapper.Object, _mockUserRepository.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldThrow_WhenCartNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var itemDto = new CartItemCreateDto { ProductId = Guid.NewGuid(), Quantity = 1 };

            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync((Cart)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cartService.AddItemToCartAsync(cartId, itemDto));
        }

        [Fact]
        public async Task ClearCartAsync_ShouldClearCart_WhenCartExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(Guid.NewGuid());
            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _cartService.ClearCartAsync(cartId);

            // Assert
            Assert.True(result);
            Assert.Empty(cart.CartItems!);
            _mockCartRepository.Verify(x => x.UpdateAsync(cart), Times.Once);
        }

        [Fact]
        public async Task ClearCartAsync_ShouldThrow_WhenCartNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync((Cart)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cartService.ClearCartAsync(cartId));
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldReturnCart_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var cart = new Cart(userId);

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(userId)).ReturnsAsync(cart);
            _mockMapper.Setup(x => x.Map<CartReadDto>(cart)).Returns(new CartReadDto());

            // Act
            var result = await _cartService.GetCartByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            _mockCartRepository.Verify(x => x.GetCartByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(userId)).ReturnsAsync((Cart)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cartService.GetCartByUserIdAsync(userId));
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 3;

            var cart = new Cart(cartId);
            var cartItem = new CartItem(cartId, productId, quantity) { Id = itemId };
            cart.AddItem(cartItem);

            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // Act
            var result = await _cartService.RemoveItemFromCartAsync(cartId, itemId);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.UpdateAsync(cart), Times.Once);
        }

        [Fact]
        public async Task RemoveItemFromCartAsync_ShouldThrow_WhenItemNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var cart = new Cart(Guid.NewGuid());
            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _cartService.RemoveItemFromCartAsync(cartId, itemId));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteCartAsync_ShouldReturnCorrectResult_WhenCartExists(bool expectedResult)
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId);
            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockCartRepository.Setup(x => x.DeleteAsync(cartId)).ReturnsAsync(expectedResult);

            // Act
            var result = await _cartService.DeleteOneAsync(cartId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockCartRepository.Verify(x => x.DeleteAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldReturnFalse_WhenDeletionFails()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(r => r.DeleteAsync(cartId)).ReturnsAsync(false);

            // Act
            var result = await _cartService.DeleteOneAsync(cartId);

            // Assert
            Assert.False(result);
            _mockCartRepository.Verify(r => r.DeleteAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task UpdateCartAsync_ThrowsKeyNotFoundException_WhenCartNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync((Cart)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cartService.UpdateOneAsync(cartId, new CartUpdateDto()));
        }

        [Fact]
        public async Task UpdateCartAsync_ReturnsUpdatedDto_WhenUpdateIsSuccessful()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart();
            var updatedCart = new Cart();
            var cartReadDto = new CartReadDto();

            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.UpdateAsync(It.IsAny<Cart>())).ReturnsAsync(updatedCart);
            _mockMapper.Setup(m => m.Map<CartUpdateDto, Cart>(It.IsAny<CartUpdateDto>(), cart)).Callback<CartUpdateDto, Cart>((dto, c) => { });
            _mockMapper.Setup(m => m.Map<CartReadDto>(updatedCart)).Returns(cartReadDto);

            // Act
            var result = await _cartService.UpdateOneAsync(cartId, new CartUpdateDto());

            // Assert
            Assert.Equal(cartReadDto, result);
            _mockMapper.Verify(m => m.Map<CartUpdateDto, Cart>(It.IsAny<CartUpdateDto>(), cart), Times.Once);
            _mockCartRepository.Verify(r => r.UpdateAsync(cart), Times.Once);
        }


    }
}