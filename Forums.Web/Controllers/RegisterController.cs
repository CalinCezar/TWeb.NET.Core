using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.User;
using Forums.Domain.Entities.Response;
using Forums.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Forums.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public RegisterController(BusinessLogic.Interfaces.ISession session)
        {
            _session = session;
        }

        // GET: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserRegister uRegis)
        {
            if (ModelState.IsValid)
            {
                if ((HttpContext.Session.GetInt32("VerificationCode") ?? 0) != uRegis.VerificationCode)
                {
                    ModelState.AddModelError("", "Invalid verification code!");
                    return View(uRegis);
                }

                URegisterData user = new URegisterData
                {
                    Credential = uRegis.Credential,
                    Password = uRegis.Password,
                    Email = uRegis.Email,
                    InfoBlog = uRegis.InfoBlog,
                    LoginIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                    LoginDateTime = DateTime.Now
                };


                GeneralResp resp = await _session.RegisterNewUserActionAsync(user);

                if (resp.Status)
                {
                    // ADD COOKIE
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", resp.StatusMsg);
                    return View(uRegis);
                }
            }
            return View(uRegis);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(string email)
        {
            Random random = new Random();
            int verificationCode = random.Next(123456, 1000000);

            GeneralResp resp = await _session.SendEmailToUserActionAsync(email, "Name", "Verification code for account registration", "Code: " + verificationCode);
            if (resp.Status)
            {
                HttpContext.Session.SetInt32("VerificationCode", verificationCode);
            }
            return Json(new { success = resp.Status });
        }
    }
}
