using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IProductService : IBaseService<ProductReadDto, ProductCreateDto, ProductUpdateDto, ProductQueryOptions>
    {
        Task<IEnumerable<ProductReadDto>> GetMostPurchased(int topNumber);

        Task<ProductReadDto> UpdateProductDetailsAsync(Guid productId, ProductUpdateDto updateDto);

        Task<ProductReadDto> UpdateProductCategoryAsync(Guid productId, Guid newCategoryId);

        Task<ProductReadDto> AdjustProductInventoryAsync(Guid productId, int adjustment);

        Task<ProductReadDto> SetProductImagesAsync(Guid productId, IEnumerable<ProductImageCreateDto> imageDtos);
    }
}