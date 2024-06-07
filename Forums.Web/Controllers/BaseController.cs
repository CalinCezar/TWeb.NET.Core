using Forums.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Forums.BusinessLogic.Interfaces;
using Forums.Web.Extension;
using System.Linq;

namespace Forums.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public BaseController(BusinessLogic.Interfaces.ISession session)
        {
            _session = session;
        }

        public void SessionStatus()
        {
            var apiCookie = Request.Cookies["X-KEY"];
            if (apiCookie != null)
            {
                var profile = _session.GetUserByCookieAsync(apiCookie);
                if (profile != null)
                {
                    HttpContext.Session.SetObject("UserProfile", profile);
                    HttpContext.Session.SetString("LoginStatus", "login");
                }
                else
                {
                    HttpContext.Session.Clear();
                    if (Request.Cookies.ContainsKey("X-KEY"))
                    {
                        Response.Cookies.Delete("X-KEY");
                    }
                    HttpContext.Session.SetString("LoginStatus", "logout");
                }
            }
            else
            {
                HttpContext.Session.SetString("LoginStatus", "logout");
            }
        }
    }
}
