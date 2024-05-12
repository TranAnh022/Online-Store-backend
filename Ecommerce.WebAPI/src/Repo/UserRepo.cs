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
    public class UserRepo : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;

        public UserRepo(AppDbContext context)
        {
            _context = context;
            _users = _context.Users;
        }

        public async Task<User> AddAsync(User entity)
        {
            _users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _users.FindAsync(id);
            if (user == null)
                return false;
            _users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _users.AnyAsync(u => u.Id == userId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _users.FindAsync(id);
        }

        public async Task<User> GetUserByCredentialsAsync(UserCredential userCredential)
        {
            var foundUser = await _users.FirstOrDefaultAsync(user => user.Email == userCredential.Email);
            if (foundUser != null)
            {
                Console.WriteLine($"User found with ID: {foundUser.Id}");
            }
            else
            {
                Console.WriteLine("No user found with the provided credentials.");
            }
            return foundUser;
        }

        public async Task<IEnumerable<User>> ListAsync(UserQueryOptions options)
        {
            Console.WriteLine($"options", options);

            if (options == null)
            {

                return await _users.ToListAsync();
            }
            IQueryable<User> query = _users;

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

        public async Task<User> RegisterAsync(User newUser)
        {
            _users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _users.FindAsync(userId);
            if (user == null)
                return false;

            user.Password = newPassword;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, UserRole newRole)
        {
            var user = await _users.FindAsync(userId);
            if (user == null)
                return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}