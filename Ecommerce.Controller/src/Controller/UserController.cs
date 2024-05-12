
using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync([FromQuery] UserQueryOptions options)
        {
            try
            {
                return await _userService.GetAllAsync(options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")] // define endpoint: /users/{id}
        public async Task<UserReadDto> GetUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetOneByIdAsync(id);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return await _userService.GetOneByIdAsync(userId);
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<UserReadDto> CreateUserAsync([FromBody] UserCreateDto dto)
        {
            var user = await _userService.CreateOneAsync(dto);
            return user;
        }

        [Authorize]
        [HttpPut("{userId}")] // endpoint: /users/:user_id
        public async Task<UserReadDto> UpdateUserByIdAsync([FromRoute] Guid userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                return await _userService.UpdateOneAsync(userId, userUpdateDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")] // endpoint: /users/:user_id
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid userId)
        {
            try
            {
                return await _userService.DeleteOneAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}