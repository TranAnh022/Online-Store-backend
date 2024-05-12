using Ardalis.GuardClauses;
using Ecommerce.Core.src.Entities.CartAggregate;

namespace Ecommerce.Core.src.Entities
{
    public class Product : BaseEntity
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        public IEnumerable<ProductImage>? Images { get; set; }
        public IEnumerable<CartItem>? CartItems { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }

        // Parameterless constructor required for generics and serialization purposes
        public Product() { }

        public Product(string title, decimal price, string description, Guid categoryId, int inventory)
        {
            Id = Guid.NewGuid();
            Title = title;
            Price = price;
            Description = description;
            CategoryId = categoryId;
            Inventory = inventory;
        }

        // Updates product details
        public void UpdateDetails(string? title = null, decimal? price = null, string? description = null)
        {
            if (title != null)
            {
                Guard.Against.NullOrEmpty(title, nameof(title));
                Title = title;
            }

            if (price != null)
            {
                Guard.Against.Negative(price.Value, nameof(price));
                Price = price.Value;
            }

            if (description != null)
            {
                Guard.Against.NullOrEmpty(description, nameof(description));
                Description = description;
            }
        }

        // Updates the category of the product
        public void UpdateCategory(Guid categoryId)
        {
            Guard.Against.Null(categoryId, nameof(categoryId));
            CategoryId = categoryId;
        }

        // Adjust inventory levels
        public void AdjustInventory(int adjustment)
        {
            Guard.Against.OutOfRange(Inventory + adjustment, nameof(Inventory), 0, int.MaxValue, "Adjustment would result in negative inventory.");
            Inventory += adjustment;
        }

        // Set product images
        public void SetImages(IEnumerable<ProductImage> images)
        {
            Guard.Against.Null(images, nameof(images));
            Images = images;
        }

    }
}