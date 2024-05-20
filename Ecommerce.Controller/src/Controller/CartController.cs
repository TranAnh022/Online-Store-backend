using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        // GET: api/v1/carts
        // Get all carts
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCarts([FromQuery] QueryOptions options)
        {
            var carts = await _cartService.GetAllAsync(options);
            return Ok(carts);
        }

        // GET: api/v1/carts/userCart
        // Get cart by id
        [HttpGet("userCart")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            Console.WriteLine($"userID {userId}");
            var cart = await _cartService.GetCartByUserIdAsync(Guid.Parse(userId));
            return Ok(cart);
        }

        // Cart items endpoints
        // GET: api/v1/carts/{cartId}/cartItems/{itemId}
        // Get cart item by id
        [HttpGet("{cartId}/cartItems/{itemId}")]
        [Authorize]
        public async Task<IActionResult> GetCartItem([FromRoute] Guid itemId)
        {
            var item = await _cartItemService.GetOneByIdAsync(itemId);
            return Ok(item);
        }

        // POST: api/v1/carts/
        // Create a new cart item
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemCreateDto cartItemCreateDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var cart = await _cartService.AddItemToCartAsync(cartItemCreateDto.ProductId, cartItemCreateDto.Quantity, Guid.Parse(userId));
            return Ok(cart);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveCartItem([FromBody] CartItemCreateDto cartItemCreateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var cart = await _cartService.RemoveItemFromCartAsync(cartItemCreateDto.ProductId, cartItemCreateDto.Quantity, Guid.Parse(userId));
            return Ok(cart);
        }
    }
}