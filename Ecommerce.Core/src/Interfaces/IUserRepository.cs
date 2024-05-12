using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.ValueObjects;


namespace Ecommerce.Core.src.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, UserQueryOptions>
    {
        // Register a new user
        Task<User> RegisterAsync(User newUser);

        // Get a user by their email address
        Task<User> GetByEmailAsync(string email);

        // Update a user's role
        Task<bool> UpdateUserRoleAsync(Guid userId, UserRole newRole);

        // Reset a user's password
        Task<bool> ResetPasswordAsync(Guid userId, string newPassword);

        // Check if a user exists
        Task<bool> ExistsAsync(Guid userId);

        Task<User> GetUserByCredentialsAsync(UserCredential userCredential);

    }

}