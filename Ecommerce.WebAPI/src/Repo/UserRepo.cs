using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.WebAPI.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class UserRepo : BaseRepo<User,UserQueryOptions>, IUserRepository
    {

        public UserRepo(AppDbContext context) :base(context)
        {
        }

        public async Task<IEnumerable<User>> ListAsync(UserQueryOptions options)
        {
            Console.WriteLine($"options", options);

            if (options == null)
            {

                return await _data.ToListAsync();
            }

            var query = _data.AsQueryable();

            if (!string.IsNullOrEmpty(options.Name))
            {
                query = query.Where(u => u.Name == options.Name);
            }

            if (!string.IsNullOrEmpty(options.Role) && Enum.TryParse<UserRole>(options.Role, out var role))
            {
                query = query.Where(u => u.Role == role);
            }

            var users = await query.ToListAsync();


            return users;
        }

        public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _data.FindAsync(userId);
            if (user == null)
                return false;

            user.Password = newPassword;
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _data.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}