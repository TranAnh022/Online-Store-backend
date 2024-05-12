using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class CategoryService : BaseService<Category, CategoryReadDto, CategoryCreateDto, CategoryUpdateDto, QueryOptions>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
            : base(categoryRepository, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryReadDto> UpdateCategoryNameAsync(Guid categoryId, string newName)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new KeyNotFoundException("Category not found");
            category.UpdateName(newName);
            await _categoryRepository.UpdateAsync(category);
            return _mapper.Map<CategoryReadDto>(category);
        }

        public async Task<CategoryReadDto> UpdateCategoryImageAsync(Guid categoryId, string newImageUrl)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new KeyNotFoundException("Category not found");
            category.UpdateImage(newImageUrl);
            await _categoryRepository.UpdateAsync(category);
            return _mapper.Map<CategoryReadDto>(category);
        }
    }
}