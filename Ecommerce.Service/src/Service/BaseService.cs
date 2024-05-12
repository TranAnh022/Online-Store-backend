using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.Service;

namespace Ecommerce.Service.src.Service
{
    public class BaseService<TEntity, TReadDTO, TCreateDTO, TUpdateDTO, TQueryOptions> : IBaseService<TReadDTO, TCreateDTO, TUpdateDTO, TQueryOptions>
    where TEntity : BaseEntity, new()
    where TQueryOptions : QueryOptions
    {
        protected readonly IBaseRepository<TEntity, TQueryOptions> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity, TQueryOptions> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TReadDTO> CreateOneAsync(TCreateDTO createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            entity = await _repository.AddAsync(entity);
            return _mapper.Map<TReadDTO>(entity);
        }

        public virtual async Task<bool> DeleteOneAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public virtual async Task<IEnumerable<TReadDTO>> GetAllAsync(TQueryOptions options)
        {
            var entities = await _repository.ListAsync(options);
            return _mapper.Map<IEnumerable<TReadDTO>>(entities);
        }

        public virtual async Task<TReadDTO> GetOneByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Entity not found");
            return _mapper.Map<TReadDTO>(entity);
        }

        public virtual async Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO update)
        {

            var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Entity not found for update");
            
            _mapper.Map(update, entity);
            Console.WriteLine($"Entity before update: {entity}");

            foreach (var property in update!.GetType().GetProperties())
            {
                var propertyName = property.Name;

                var propertyValue = property.GetValue(update);

                // Find the corresponding property on the entity object
                var entityProperty = entity.GetType().GetProperty(propertyName);

                // Check if the entity property exists and is writable
                if (entityProperty != null && entityProperty.CanWrite)
                {
                    // Set the value of the entity property
                    entityProperty.SetValue(entity, propertyValue);

                }
            }

            Console.WriteLine($"Entity after update: {entity}");

            entity = await _repository.UpdateAsync(entity);
            return _mapper.Map<TReadDTO>(entity);
        }
    }
}