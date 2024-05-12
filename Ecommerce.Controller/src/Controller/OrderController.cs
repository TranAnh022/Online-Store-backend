using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly IOrderItemService _orderItemService;
        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync([FromQuery] QueryOptions options)
        {
            try
            {
                return await _orderService.GetAllAsync(options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<OrderReadDto> GetOrderByIdAsync([FromRoute] Guid id)
        {
            return await _orderService.GetOneByIdAsync(id);
        }

        [Authorize]
        [HttpPost()]
        public async Task<OrderReadDto> CreateOrderAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (userId == null)
            {
                throw new Exception("User not found");
            }
            return await _orderService.CreateOrderFromCartAsync(Guid.Parse(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<OrderReadDto> UpdateOrderByIdAsync([FromRoute] Guid id, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            try
            {
                return await _orderService.UpdateOneAsync(id, orderUpdateDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrderByIdAsync([FromRoute] Guid id)
        {
            try
            {
                return await _orderService.DeleteOneAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IEnumerable<OrderReadDto>> GetAllOrderByUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new Exception("User not found");
                }
                var orders = await _orderService.GetOrdersByUserIdAsync(Guid.Parse(userId));

                return orders;
            }
            catch (Exception ex)
            {
                // You can return a specific HTTP status code along with the error message
                throw new Exception($"Failed to retrieve orders for the user: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("cancel-order/{id}")]
        public async Task<bool> CancelOrder([FromRoute] Guid id)
        {
            var order = await _orderService.GetOneByIdAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (order is null)
            {
                throw new Exception($"Order not found {id}");
            }
            if (userId != order.UserId.ToString())
            {
                throw new Exception("You don't have permission to cancel order");
            }
            return await _orderService.CancelOrderAsync(id);
        }

        [HttpGet("orderItem")]
        public async Task<IEnumerable<OrderItemReadDto>> GetAllOrderItem(QueryOptions queryOptions)
        {
            return await _orderItemService.GetAllAsync(queryOptions);
        }


    }
}