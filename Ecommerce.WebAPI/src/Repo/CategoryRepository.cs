using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.WebAPI.src.Data;
using Ecommerce.WebAPI.src.Repo;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repository
{
    public class CategoryRepository :BaseRepo<Category,QueryOptions>,ICategoryRepository
    {
        public CategoryRepository(AppDbContext context):base(context)
        {
        }
    }
}