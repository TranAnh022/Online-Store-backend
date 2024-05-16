
using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [Route("api/v1/users")]
    public class UserController : ControllerBase

    {
        private readonly IUserService _userService;
        private IAuthorizationService _authorizationService;
        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] QueryOptions options)
        {
            return Ok(await _userService.GetAllAsync(options));
        }

        [Authorize]
        [HttpGet("{id}")] // define endpoint: /users/{id}
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _userService.GetOneByIdAsync(id));
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _userService.GetOneByIdAsync(userId));
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto dto)
        {
            var user = await _userService.CreateOneAsync(dto);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{userId}")] // endpoint: /users/:user_id
        public async Task<IActionResult> UpdateUserByIdAsync([FromRoute] Guid userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _userService.GetOneByIdAsync(userId);
            var authorizationResult = _authorizationService
           .AuthorizeAsync(HttpContext.User, user, "AdminOrOwnerAccount")
           .GetAwaiter()
           .GetResult();

            if (authorizationResult.Succeeded)
            {
                var userUpdated = await _userService.UpdateOneAsync(userId, userUpdateDto);
                return Ok(userUpdated);
            }
            else
            {
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{userId}")] // endpoint: /users/:user_id
        public async Task<IActionResult> DeleteUserByIdAsync([FromRoute] Guid userId)
        {
            var user = await _userService.GetOneByIdAsync(userId);

            var authorizationResult = _authorizationService
            .AuthorizeAsync(HttpContext.User, user, "AdminOrOwnerAccount")
            .GetAwaiter()
            .GetResult();
            if (authorizationResult.Succeeded)
            {
                await _userService.DeleteOneAsync(userId);
            }
            return Ok(true);
        }
    }
}