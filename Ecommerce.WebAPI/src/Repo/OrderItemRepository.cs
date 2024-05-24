using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class OrderItemRepository :BaseRepo<OrderItem,QueryOptions>,IBaseRepository<OrderItem, QueryOptions>
    {

        public OrderItemRepository(AppDbContext context):base(context)
        {

        }

        public override async Task<IEnumerable<OrderItem>> ListAsync(QueryOptions queryOptions)
        {
            var query = _data.AsQueryable();

            if (queryOptions.Page.HasValue && queryOptions.PageSize.HasValue)
            {
                query = query.Skip((queryOptions.Page.Value - 1) * queryOptions.PageSize.Value).Take(queryOptions.PageSize.Value);
            }
            return await query.ToListAsync();
        }

    }
}