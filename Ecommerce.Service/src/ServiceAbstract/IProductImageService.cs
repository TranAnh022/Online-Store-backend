using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.Service;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IProductImageService : IBaseService<ProductImageReadDto, ProductImageCreateDto, ProductImageUpdateDto, QueryOptions>
    {
        Task<ProductImageReadDto> UpdateImageUrlAsync(Guid imageId, string newUrl);
    }
}