using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ICloudinaryService
    {
        Task<string> AddPhoto(IFormFile file);
        Task<bool> DeletePhoto(Guid Id);
    }
}