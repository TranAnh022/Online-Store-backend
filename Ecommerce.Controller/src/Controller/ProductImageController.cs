using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/product/productImages")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct([FromQuery] QueryOptions options)
        {
            return Ok(await _productImageService.GetAllAsync(options));
        }

        [HttpGet("{productImageId}")]
        public async Task<IActionResult> GetProductImageByIdAsync([FromRoute] Guid productImageId)
        {
              return Ok(await _productImageService.GetOneByIdAsync(productImageId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateImage(ProductImageCreateDto img)
        {
            return Ok(await _productImageService.CreateOneAsync(img));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productImageId}")]
        public async Task<IActionResult> UpdateProductImageByIdAsync([FromRoute] Guid productImageId, [FromBody] ProductImageUpdateDto productImageUpdateDto)
        {
            return Ok(await _productImageService.UpdateOneAsync(productImageId, productImageUpdateDto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productImageId}")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromRoute] Guid productImageId)
        {
            return Ok(await _productImageService.DeleteOneAsync(productImageId));
        }
    }
}