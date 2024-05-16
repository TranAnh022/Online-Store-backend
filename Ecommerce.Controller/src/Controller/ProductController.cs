using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;


        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<ProductReadDto>> GetAllProduct([FromQuery] ProductQueryOptions options)
        {
            return await _productService.GetAllAsync(options);
        }

        [HttpGet("{productId}")]
        public async Task<ProductReadDto> GetProductByIdAsync([FromRoute] Guid productId)
        {

            return await _productService.GetOneByIdAsync(productId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ProductReadDto> CreateProductAsync([FromForm] ProductCreateDto productCreateDto)
        {
            return await _productService.CreateOneAsync(productCreateDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<ProductReadDto> UpdateProductByIdAsync([FromRoute] Guid productId, [FromBody] ProductUpdateDto productUpdateDto)
        {

            return await _productService.UpdateOneAsync(productId, productUpdateDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid productId)
        {
            return await _productService.DeleteOneAsync(productId);
        }
    }
}