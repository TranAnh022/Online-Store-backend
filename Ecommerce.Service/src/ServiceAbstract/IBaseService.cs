using Ecommerce.Core.src.Common;

namespace Ecommerce.Service.Service
{
    public interface IBaseService<TReadDTO, TCreateDTO, TUpdateDTO, TQueryOptions>
         where TQueryOptions : QueryOptions
    {
        Task<IEnumerable<TReadDTO>> GetAllAsync(TQueryOptions options);
        Task<TReadDTO> GetOneByIdAsync(Guid id);
        Task<TReadDTO> CreateOneAsync(TCreateDTO createDto);
        Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updateDto);
        Task<bool> DeleteOneAsync(Guid id);
    }

}