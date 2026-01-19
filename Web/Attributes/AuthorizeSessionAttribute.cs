using Application.Services.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeSessionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly ISessionManagerService _sessionManager;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionManager =
                (ISessionManagerService)context.HttpContext.RequestServices.GetService(typeof(ISessionManagerService))!;
            var userSession = sessionManager.Get<int>("CurrentUser");

            if (userSession > 0) return;

            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }
}
