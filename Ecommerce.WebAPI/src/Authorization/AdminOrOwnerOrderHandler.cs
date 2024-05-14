using System.Security.Claims;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;


namespace Walmad.WebAPI.src.Authorization
{
    public class AdminOrOwnerOrderHandler : AuthorizationHandler<AdminOrOwnerOrderRequirement, OrderReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerOrderRequirement requirement, OrderReadDto order)
        {
            var claims = context.User;
            var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
            var userId = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (userId == order.UserId.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class AdminOrOwnerOrderRequirement : IAuthorizationRequirement { }
}

