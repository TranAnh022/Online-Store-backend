
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.ValueObjects;

namespace Ecommerce.Service.src.DTO
{
    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemReadDto>? OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();
        public OrderStatus Status { get; set; }
    }

    public class OrderUpdateDto
    {
        public OrderStatus? Status { get; set; }
        public List<OrderItemUpdateDto>? ItemsToUpdate { get; set; }
    }

}