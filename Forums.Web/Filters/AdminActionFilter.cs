using Forums.BusinessLogic.Interfaces;
using Forums.Web.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Forums.Web.Filters
{
    public class AdminActionFilter : ActionFilterAttribute
    {
        private readonly IUser _user;
        private readonly IMySession _session;

        public AdminActionFilter(IUser user, IMySession session)
        {
            _user = user;
            _session = session;
        }

        public override async void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as Controller;
            var sessionTask = _session.GetSessionByCookieAsync(context.HttpContext.Request.Cookies["X-Key"]);
            var session = await sessionTask;
            if (session != null)
            {
                var profileTask = _user.GetUserBySessionAsync(session);
                var profile = await profileTask;
                if (profile != null && profile.Level == Domain.Enum.UserRole.Admin)
                {
                    controller.ViewBag.IsAdmin = true;
                }
                else
                {
                    controller.ViewBag.IsAdmin = false;
                }
            }
            else
            {
                controller.ViewBag.IsAdmin = false;
            }
        }


    }
}
