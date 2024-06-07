using Forums.BusinessLogic.Interfaces;
using Forums.Web.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Forums.Web.Filters
{
    public class AdminActionFilter : IAsyncActionFilter
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public AdminActionFilter(BusinessLogic.Interfaces.ISession session)
        {
            _session = session;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var apiCookie = context.HttpContext.Request.Cookies["X-Key"];
            if (apiCookie != null)
            {
                var profile = await _session.GetUserByCookieAsync(apiCookie);
                if (profile != null && profile.Level == Domain.Enum.UserRole.Admin)
                {
                    context.HttpContext.SetMySessionObject(profile);
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "HomePage" }));
                    return;
                }
            }
            await next();
        }
    }
}
