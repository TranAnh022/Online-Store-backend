using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ICategoryService : IBaseService<CategoryReadDto, CategoryCreateDto, CategoryUpdateDto, QueryOptions>
    {
        Task<CategoryReadDto> UpdateCategoryNameAsync(Guid categoryId, string newName);

        Task<CategoryReadDto> UpdateCategoryImageAsync(Guid categoryId, string newImageUrl);
    }
}