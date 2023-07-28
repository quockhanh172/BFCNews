using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BFCNews.Service
{
    public class AccessDeniedAuthorizeAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, redirect them to the login page
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
            else
            {
                // If the user is authenticated but does not have the required role, redirect them to the access denied page
                if (Roles != null && !context.HttpContext.User.IsInRole(Roles))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
                }
            }
        }
    }
}
