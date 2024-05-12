using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities.CartAggregate
{
    public class CartItem : TimeStamp
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }

        public CartItem() { }
        public CartItem(Guid cartId, Guid productId, int quantity)
        {
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }
        public void AddQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity += quantity;
        }

        public void SetQuantity(int quantity)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);

            Quantity = quantity;
        }


    }
}