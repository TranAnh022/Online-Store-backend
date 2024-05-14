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
        public void AddItem(Guid cartId, Product product, int quantity)
        {
            var existingItem = _items?.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                // If the item already exists, update its quantity
                existingItem.AddQuantity(quantity);
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                var cartItem = new CartItem(cartId, product.Id, quantity);
                _items?.Add(cartItem);
            }
        }

        // Method for remove item from cart
        public void RemoveItem(Product product, int quantity)
        {
            var item = _items.FirstOrDefault(item => item.ProductId == product.Id);

            if (item == null) return;

            item.Quantity -= quantity;

            if (item.Quantity == 0) _items.Remove(item);

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
