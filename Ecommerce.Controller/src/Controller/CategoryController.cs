using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/v1/categories
        // Retrieves all categories
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetAllCategories([FromQuery] QueryOptions options)
        {
            var categories = await _categoryService.GetAllAsync(options);
            return Ok(categories);
        }

        // GET: api/v1/categories/{id}
        // Retrieves a specific category by ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetCategoryById([FromRoute] Guid id)
        {
            try
            {
                var category = await _categoryService.GetOneByIdAsync(id);
                if (category == null)
                    return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/v1/categories
        // Creates a new category
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryReadDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var createdCategory = await _categoryService.CreateOneAsync(categoryCreateDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/v1/categories/{id}
        // Updates a specific category
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateOneAsync(id, categoryUpdateDto);
                if (updatedCategory == null)
                    return NotFound();
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/v1/categories/{id}
        // Deletes a specific category
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                var success = await _categoryService.DeleteOneAsync(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/v1/categories/{id}/updated-name
        // Updates the name of a specific category
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/updated-name")]
        public async Task<ActionResult<CategoryReadDto>> UpdateCategoryName([FromRoute] Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryNameAsync(id, categoryUpdateDto.Name!);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/v1/categories/{id}/updated-image
        // Updates the image of a specific category
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:guid}/updated-image")]
        public async Task<ActionResult<CategoryReadDto>> UpdateCategoryImage([FromRoute] Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryImageAsync(id, categoryUpdateDto.Image!);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}