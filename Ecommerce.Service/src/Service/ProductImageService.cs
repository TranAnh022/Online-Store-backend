using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class ProductImageService : BaseService<ProductImage, ProductImageReadDto, ProductImageCreateDto, ProductImageUpdateDto, QueryOptions>, IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IProductImageRepository productImageRepository, IMapper mapper)
            : base(productImageRepository, mapper)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<ProductImageReadDto> UpdateImageUrlAsync(Guid imageId, string newUrl)
        {
            var productImage = await _productImageRepository.GetByIdAsync(imageId) ?? throw new KeyNotFoundException("Product image not found");
            productImage.UpdateUrl(newUrl);
            await _productImageRepository.UpdateAsync(productImage);
            return _mapper.Map<ProductImageReadDto>(productImage);
        }
    }
}