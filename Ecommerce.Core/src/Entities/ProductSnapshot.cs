using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.ValueObjects
{
    public class ProductSnapshot
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }

        public ProductSnapshot() { }
        public ProductSnapshot(Guid productId, string title, decimal price, string description)
        {
            Guard.Against.Default(productId, nameof(productId), "Product ID cannot be the default GUID.");
            Guard.Against.NullOrEmpty(title, nameof(title), "Title cannot be null or empty.");
            Guard.Against.NegativeOrZero(price, nameof(price), "Price must be greater than zero.");
            Guard.Against.NullOrEmpty(description, nameof(description), "Description cannot be null or empty.");

            Id = Guid.NewGuid();
            ProductId = productId;
            Title = title;
            Price = price;
            Description = description;
        }


    }
}