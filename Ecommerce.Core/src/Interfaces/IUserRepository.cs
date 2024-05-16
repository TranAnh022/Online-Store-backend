using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.ValueObjects;


namespace Ecommerce.Core.src.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, QueryOptions>
    {
        Task<bool> ResetPasswordAsync(Guid userId, string newPassword);

        Task<User> GetByEmailAsync(string email);
    }

}