using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public IEnumerable<Product>? Products { get; set; }

        public Category() { }

        public Category(string name, string image)
        {
            Id = Guid.NewGuid();
            Name = name;
            Image = image;
        }

        // Method to update the category name
        public void UpdateName(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name), "Category name cannot be null or empty.");
            Name = name;
        }

        // Method to update the category image URL
        public void UpdateImage(string image)
        {
            Guard.Against.NullOrEmpty(image, nameof(image), "Image URL cannot be null or empty.");
            Guard.Against.InvalidInput(image, nameof(image), uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute), "Image URL must be a valid URL.");
            Image = image;
        }
    }
}