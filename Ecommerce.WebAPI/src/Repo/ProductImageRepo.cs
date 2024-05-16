using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ProductImageRepo : BaseRepo<ProductImage,QueryOptions>,IProductImageRepository
    {

        public ProductImageRepo(AppDbContext context):base(context)
        {
        }

        public async Task<bool> CheckImageAsync(string imageUrl)
        {
            return await _data.AnyAsync(p => p.Url == imageUrl);
        }

        public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(Guid productId)
        {
            return await _data.Where(p => p.ProductId == productId).ToListAsync();
        }
    }
}