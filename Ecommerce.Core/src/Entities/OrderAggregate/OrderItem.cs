using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using Ecommerce.Core.src.ValueObjects;

namespace Ecommerce.Core.src.Entities.OrderAggregate
{
    public class OrderItem : TimeStamp
    {
        public Guid OrderId { get; set; }
        public Guid ProductSnapshotId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Order? Order { get; set; }
        public ProductSnapshot? ProductSnapshot { get; set; }

        public OrderItem() { }

        public OrderItem(Guid orderId, Guid productSnapshotId, int quantity, decimal price)
        {
            Guard.Against.Default(orderId, nameof(orderId));
            Guard.Against.Null(productSnapshotId, nameof(productSnapshotId));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Id = Guid.NewGuid();
            OrderId = orderId;
            ProductSnapshotId = productSnapshotId;
            Quantity = quantity;
            Price = price;
        }
    }
}