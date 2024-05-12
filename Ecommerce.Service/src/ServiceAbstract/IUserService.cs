using Ecommerce.Core.src.Common;
using Ecommerce.Service.DTO;
using Ecommerce.Service.Service;


namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IUserService : IBaseService<UserReadDto, UserCreateDto, UserUpdateDto, UserQueryOptions>
    {
        // Adds a method to update the user's password
        Task<bool> UpdatePasswordAsync(Guid userId, string newPassword);

        // Adds a method to update the user's role
        Task<UserReadDto> UpdateRoleAsync(Guid userId, UserRoleUpdateDto roleUpdateDto);

        // Adds a method to handle specific search or filter operations
        Task<IEnumerable<UserReadDto>> SearchUsersAsync(UserQueryOptions options);
    }
}