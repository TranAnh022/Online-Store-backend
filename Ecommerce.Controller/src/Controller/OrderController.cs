using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly IAuthorizationService _authorizationService;

        private readonly IOrderItemService _orderItemService;
        public OrderController(IOrderService orderService, IOrderItemService orderItemService, IAuthorizationService authorizationService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrdersAsync([FromQuery] QueryOptions options)
        {
            return Ok(await _orderService.GetAllAsync(options));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderReadDto>> GetOrderByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.GetOneByIdAsync(id));
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<OrderReadDto>> CreateOrderAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return Ok(await _orderService.CreateOrderFromCartAsync(Guid.Parse(userId)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderReadDto>> UpdateOrderByIdAsync([FromRoute] Guid id, [FromBody] OrderUpdateDto orderUpdateDto)
        {

            return Ok(await _orderService.UpdateOneAsync(id, orderUpdateDto));

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.DeleteOneAsync(id));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrderByUser([FromQuery] string? status)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orders = await _orderService.GetOrdersByUserIdAsync(Guid.Parse(userId),status);

            return Ok(orders);

        }

        [Authorize]
        [HttpPatch("cancel-order/{id}")]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid id)
        {
            var foundOrder = await _orderService.GetOneByIdAsync(id);
            if (foundOrder is null)
            {
                throw CustomExeption.NotFoundException("Order not found");
            }
            else
            {
                var authorizationResult = _authorizationService
               .AuthorizeAsync(HttpContext.User, foundOrder, "AdminOrOwnerOrder")
               .GetAwaiter()
               .GetResult();

                if (authorizationResult.Succeeded)
                {
                    return Ok(await _orderService.CancelOrderAsync(id));
                }
                else
                {
                    return new ForbidResult();
                }
            }
        }

        [HttpGet("orderItem")]
        public async Task<ActionResult<IEnumerable<OrderItemReadDto>>> GetAllOrderItem(QueryOptions queryOptions)
        {
            return Ok(await _orderItemService.GetAllAsync(queryOptions));
        }


    }
}