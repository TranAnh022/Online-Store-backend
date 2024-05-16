using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] UserCredential userCredential)
        {
            return Ok(await _authService.LogInAsync(userCredential));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> AuthenticateUser(string token)
        {
            var data = await _authService.AuthenticateUserAsync(token);
            return Ok(data);
        }
    }
}