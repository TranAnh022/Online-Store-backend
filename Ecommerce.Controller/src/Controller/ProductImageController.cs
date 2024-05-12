using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/product/productImage")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductImageReadDto>> GetAllProduct([FromQuery] QueryOptions options)
        {
            return await _productImageService.GetAllAsync(options);
        }

        [HttpGet("{productImageId}")]
        public async Task<ProductImageReadDto> GetProductImageByIdAsync([FromRoute] Guid productImageId)
        {
            try
            {
                return await _productImageService.GetOneByIdAsync(productImageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ProductImageReadDto> CreateImage(ProductImageCreateDto img)
        {
            return await _productImageService.CreateOneAsync(img);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productImageId}")]
        public async Task<ProductImageReadDto> UpdateProductImageByIdAsync([FromRoute] Guid productImageId, [FromBody] ProductImageUpdateDto productImageUpdateDto)
        {
            try
            {
                return await _productImageService.UpdateOneAsync(productImageId, productImageUpdateDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productImageId}")]
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid productImageId)
        {
            try
            {
                return await _productImageService.DeleteOneAsync(productImageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}