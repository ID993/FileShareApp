#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Make_a_Drop.MVC.Filters
{
    public class CustomAuthorize : TypeFilterAttribute
    {
        public CustomAuthorize() : base(typeof(CustomAuthorizeFilter))
        {
        }

        private class CustomAuthorizeFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
        }




    }
}
