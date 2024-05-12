using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Service.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IAuthService
    {
        Task<string> LogInAsync(UserCredential userCredential);
        Task<User> AuthenticateUserAsync(string token);
    }
}