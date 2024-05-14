using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class CartItemRepository :BaseRepo<CartItem,QueryOptions>,IBaseRepository<CartItem, QueryOptions>
    {
        public CartItemRepository(AppDbContext context):base(context)
        {

        }
        public async Task<IEnumerable<CartItem>> ListAsync(QueryOptions options)
        {
            IQueryable<CartItem> query = _data.Include(ci => ci.Product);

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                query = query.Where(ci => ci.Product!.Title!.Contains(options.Search));
            }

            if (options.Page.HasValue && options.PageSize.HasValue)
            {
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);
            }

            return await query.ToListAsync();
        }

    }
}