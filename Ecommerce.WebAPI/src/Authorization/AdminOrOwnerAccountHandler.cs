using System.Security.Claims;
using Ecommerce.Core.src.ValueObjects;
using Microsoft.AspNetCore.Authorization;


using Ecommerce.Service.DTO;

namespace Ecommerce.WebAPI.src.Authorization
{
    public class AdminOrOwnerAccountHandler : AuthorizationHandler<AdminOrOwnerAccountRequirement, UserReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerAccountRequirement requirement, UserReadDto user)
        {
            var claims = context.User;
            var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
            var userId = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (userId == user.Id.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class AdminOrOwnerAccountRequirement : IAuthorizationRequirement { }
}