using System.Security.Claims;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;


namespace Ecommerce.WebAPI.src.Authorization
{
    public class AdminOrOwnerReviewHandler : AuthorizationHandler<AdminOrOwnerReviewRequirement, ReviewReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerReviewRequirement requirement, ReviewReadDto review)
        {
            var claims = context.User;
            var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;
            var userId = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            Console.WriteLine($"userID ${userId}");
            Console.WriteLine($"userrevew ${review.User.Id}");
            if (userId == review.User.Id.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class AdminOrOwnerReviewRequirement : IAuthorizationRequirement { }
}