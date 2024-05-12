using Ecommerce.Core.src.Common;
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

        // GET: api/v1/carts/{id}
        // Get cart by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCart([FromRoute] Guid id)
        {
            try
            {
                var cart = await _cartService.GetOneByIdAsync(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cart items endpoints
        // GET: api/v1/carts/{cartId}/cartItems/{itemId}
        // Get cart item by id
        [HttpGet("{cartId}/cartItems/{itemId}")]
        [Authorize]
        public async Task<IActionResult> GetCartItem([FromRoute] Guid cartId, [FromRoute] Guid itemId)
        {
            var cart = await _cartService.GetOneByIdAsync(cartId);
            if (cart == null) return NotFound("Cart not found.");
            try
            {
                var item = await _cartItemService.GetOneByIdAsync(itemId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/v1/carts/{cartId}/cartItems
        // Create a new cart item
        [HttpPost("{cartId}/cartItems")]
        [Authorize]
        public async Task<IActionResult> CreateCartItem([FromRoute] Guid cartId, [FromBody] CartItemCreateDto itemDto)
        {
            // Ensure cartId is correctly received
            if (cartId == Guid.Empty)
                return BadRequest("Invalid Cart ID.");
            try
            {
                var newItem = await _cartService.AddItemToCartAsync(cartId, itemDto);
                return CreatedAtAction(nameof(GetCartItem), new { cartId, itemId = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/v1/carts/{cartId}/cartItems/{itemId}
        [HttpPut("{cartId}/cartItems/{itemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem([FromRoute] Guid cartId, [FromRoute] Guid itemId, [FromBody] CartItemUpdateDto itemDto)
        {
            var cart = await _cartService.GetOneByIdAsync(cartId);
            if (cart == null) return NotFound("Cart not found.");
            try
            {
                var updatedItem = await _cartItemService.UpdateOneAsync(itemId, itemDto);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/v1/carts/{cartId}/cartItems/{itemId}
        // Delete a cart item
        [HttpDelete("{cartId}/cartItems/{itemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItem([FromRoute] Guid cartId, [FromRoute] Guid itemId)
        {
            var cart = await _cartService.GetOneByIdAsync(cartId);
            if (cart == null) return NotFound("Cart not found.");
            var result = await _cartItemService.DeleteOneAsync(itemId);
            if (!result) return NotFound("Cart item not found.");
            return NoContent();
        }
    }
}