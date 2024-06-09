using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Forums.BusinessLogic.Interfaces;
using Forums.Web.Extension;

public class SessionStatusFilter : IAsyncActionFilter
{
    private readonly Forums.BusinessLogic.Interfaces.ISession _session;

    public SessionStatusFilter(Forums.BusinessLogic.Interfaces.ISession session)
    {
        _session = session;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var apiCookie = context.HttpContext.Request.Cookies["X-KEY"];
        if (apiCookie != null)
        {
            var profile = await _session.GetUserByCookieAsync(apiCookie);
            if (profile != null)
            {
                context.HttpContext.SetMySessionObject(profile);
                context.HttpContext.Session.SetString("LoginStatus", "login");
            }
            else
            {
                context.HttpContext.Session.Clear();
                if (context.HttpContext.Request.Cookies.ContainsKey("X-KEY"))
                {
                    context.HttpContext.Response.Cookies.Delete("X-KEY");
                }

                context.HttpContext.Session.SetString("LoginStatus", "logout");
            }
        }
        else
        {
            context.HttpContext.Session.SetString("LoginStatus", "logout");
        }

        await next();
    }
}
