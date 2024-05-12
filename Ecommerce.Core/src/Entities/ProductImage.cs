using Ardalis.GuardClauses;

namespace Ecommerce.Core.src.Entities
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string? Url { get; set; }
        public Product? Product { get; set; }
        public ProductImage() { }

        public ProductImage(Guid productId, string url)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Url = url;
        }

        public void UpdateUrl(string newUrl)
        {
            Guard.Against.NullOrEmpty(newUrl, nameof(newUrl), "URL cannot be null or empty.");
            Guard.Against.InvalidInput(newUrl, nameof(newUrl),
                uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute) && (uri.StartsWith("http://") || uri.StartsWith("https://")),
                "URL must be a valid, well-formed URL and start with 'http://' or 'https://'.");

            Url = newUrl;
        }
    }
}