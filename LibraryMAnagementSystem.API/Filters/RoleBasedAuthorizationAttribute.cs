using LibraryMAnagementSystem.API.ResponseHandler;
using LibraryManagementSystem.DAL.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMAnagementSystem.API.Filters
{
    public class RoleBasedAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int _requiredRole;

        public RoleBasedAuthorizationAttribute(int requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
            if (userManager == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? context.HttpContext.User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if ((int)user.UserType != _requiredRole)
            {
                context.Result = new ObjectResult(
                    new ApiResponse<object>(
                        null,
                        "You do not have permission",
                        403
                    ))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
