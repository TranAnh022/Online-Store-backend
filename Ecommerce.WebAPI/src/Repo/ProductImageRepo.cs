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
    public class ProductImageRepo : IProductImageRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<ProductImage> _productImages;

        public ProductImageRepo(AppDbContext context)
        {
            _context = context;
            _productImages = _context.ProductImages;
        }

        public async Task<ProductImage> AddAsync(ProductImage entity)
        {
            Console.WriteLine(entity.Url);
            _productImages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> CheckImageAsync(string imageUrl)
        {
            return await _productImages.AnyAsync(p => p.Url == imageUrl);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var productImage = await _productImages.FindAsync(id);

            if (productImage == null || productImage.ProductId != Guid.Empty)
                return false;
            _productImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductImage> GetByIdAsync(Guid id)
        {
            return await _productImages.FindAsync(id);
        }

        public async Task<IEnumerable<ProductImage>> ListAsync(QueryOptions options)
        {
            return await _productImages.ToListAsync();
        }

        public async Task<ProductImage> UpdateAsync(ProductImage entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}