using Ardalis.GuardClauses;
using Ecommerce.Core.src.ValueObjects;

namespace Ecommerce.Core.src.Entities.OrderAggregate
{
    public class Order : TimeStamp
    {
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }

        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so OrderItems cannot be added from "outside the Aggregate" directly to the collection,
        // but only through the method Order.AddItem() which includes behavior.
        private readonly HashSet<OrderItem>? _orderItems;
        public IReadOnlyCollection<OrderItem>? OrderItems => _orderItems;
        public User? User { get; set; }

        public Order() { }

        public Order(Guid userId)
        {
            Guard.Against.Default(userId, nameof(userId));

            Id = Guid.NewGuid();
            UserId = userId;
            Status = OrderStatus.Pending;
            _orderItems = new HashSet<OrderItem>(new OrderItemComparer());
        }

        // Method to add or update quantity an item in the order
        public void AddOrUpdateItem(OrderItem item)
        {
            Guard.Against.Null(item, nameof(item));
            var existingItem = _orderItems?.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
            }
            else
            {
                _orderItems?.Add(item);
            }
            CalculateTotalPrice();
        }

        // Method to remove an item from the order
        public bool RemoveItem(Guid itemId)
        {
            var item = _orderItems?.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                _orderItems?.Remove(item);
                // Recalculate total price whenever an item is removed
                CalculateTotalPrice();
                return true;
            }
            return false;
        }

        // Method to update the status of the order
        public void UpdateStatus(OrderStatus newStatus)
        {
            Guard.Against.OutOfRange((int)newStatus, nameof(newStatus), 0, 4); // Assuming enums are a range
            Status = newStatus;
        }

        // Method to calculate the total price of the order
        public decimal CalculateTotalPrice()
        {
            return _orderItems!.Sum(item => item.Price * item.Quantity);
        }

    }

    // A custom equality comparer for OrderItem to define uniqueness in the HashSet
    public class OrderItemComparer : IEqualityComparer<OrderItem>
    {
        public bool Equals(OrderItem? x, OrderItem? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(OrderItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}