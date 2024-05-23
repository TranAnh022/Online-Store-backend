using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.ValueObjects;

namespace Ecommerce.Core.src.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order, QueryOptions>
    {
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId,string? status);

        Task<ProductSnapshot> AddProductSnapshotAsync(ProductSnapshot productSnapshot);


    }
}