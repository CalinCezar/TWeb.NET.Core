using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.User;
using Forums.Domain.Entities.Response;
using Forums.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Forums.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public LoginController(BusinessLogic.Interfaces.ISession session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLogin uLogin)
        {
            if (ModelState.IsValid)
            {
                ULoginData user = new ULoginData
                {
                    Credential = uLogin.Credential,
                    Password = uLogin.Password,
                    LoginIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                    LoginDateTime = DateTime.Now
                };

                GeneralResp resp = await _session.UserPassCheckActionAsync(user);

                if (resp.Status)
                {
                    string cookieOptions = await _session.GenCookieAsync(uLogin.Credential);
                    if (cookieOptions != null) { Response.Cookies.Append("X-KEY",cookieOptions); }
                    HttpContext.Session.SetString("LoginStatus", "login");
                    return RedirectToAction("HomePage", "Home");
                }
                else
                {
                    ModelState.AddModelError("", resp.StatusMsg);
                    return View();
                }
            }
            return View();
        }
    }
}
