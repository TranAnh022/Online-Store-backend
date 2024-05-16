using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;

namespace Ecommerce.Core.src.Interfaces
{
    public interface IProductImageRepository : IBaseRepository<ProductImage, QueryOptions>
    {
        Task<bool> CheckImageAsync(string imageUrl);

        Task<IEnumerable<ProductImage>> GetByProductIdAsync(Guid productId);
    }
}