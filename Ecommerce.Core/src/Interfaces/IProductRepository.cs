using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;

namespace Ecommerce.Core.src.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product, ProductQueryOptions>
    {
        // Get the top purchased products
        Task<IEnumerable<Product>> GetMostPurchasedProductsAsync(int topNumber);

        // Get images for a specific product
        Task<IEnumerable<ProductImage>> GetProductImagesAsync(Guid productId);

        // Check if product exists
        Task<bool> ExistsAsync(Guid productId);
    }
}