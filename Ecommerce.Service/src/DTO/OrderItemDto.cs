using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Service.src.DTO
{
    public class OrderItemReadDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public ProductSnapshotDto? ProductSnapshot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ProductSnapshotDto
    {
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }

    public class OrderItemCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderItemUpdateDto
    {
        public Guid ItemId { get; set; }
        public int? Quantity { get; set; }  // Nullable if only updating is needed, not replacement
    }
}