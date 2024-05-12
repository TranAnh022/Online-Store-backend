using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities.CartAggregate
{
    public class Cart : TimeStamp
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so CartItems cannot be added from "outside the Aggregate" directly to the collection,
        // but only through the method Cart.AddItem() which includes behavior.
        private readonly HashSet<CartItem>? _items;
        public ICollection<CartItem>? CartItems => _items;

        public Cart() { }

        public Cart(Guid userId)
        {
            Guard.Against.Default(userId, nameof(userId));

            Id = Guid.NewGuid();
            UserId = userId;
            _items = new HashSet<CartItem>(new CartItemComparer());
        }

        // Method for add item to cart
        public void AddItem(CartItem item)
        {
            Guard.Against.Null(item, nameof(item));
            // Check if the item already exists in the cart
            var existingItem = _items?.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                // If the item already exists, update its quantity
                existingItem.AddQuantity(item.Quantity);
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                _items?.Add(item);
            }
        }

        // Method for remove item from cart
        public bool RemoveItem(CartItem item)
        {
            return _items?.Remove(item) ?? false;
        }

        // Method for clear cart

        public void ClearCart()
        {
            _items?.Clear();
        }
    }

    // A custom equality comparer for CartItem to define uniqueness in the HashSet
    public class CartItemComparer : IEqualityComparer<CartItem>
    {
        public bool Equals(CartItem? x, CartItem? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(CartItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
