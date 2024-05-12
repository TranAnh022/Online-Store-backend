using Ecommerce.Core.src.Common;

namespace Ecommerce.Core.src.Interfaces
{
    public interface IBaseRepository<T, TQueryOptions> where T : class where TQueryOptions : QueryOptions
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> ListAsync(TQueryOptions options);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
