using System;
using System.Threading.Tasks;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using Forums.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forums.Web.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public ResetPasswordController(BusinessLogic.Interfaces.ISession session)
        {
            _session = session;
        }

        // GET: ResetPassword
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserRegister uRegis)
        {
            if (!string.IsNullOrEmpty(uRegis.Email))
            {
                GeneralResp resp = await _session.ExistingEmailInDBAsync(uRegis.Email);
                if (!resp.Status)
                {
                    return View(uRegis);
                }

                HttpContext.Session.Remove("ResetToken");
                HttpContext.Session.Remove("ResetTokenExpiration");
                HttpContext.Session.Remove("Email");

                string token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("ResetToken", token);
                HttpContext.Session.SetString("ResetTokenExpiration", DateTime.Now.AddMinutes(5).ToString());
                HttpContext.Session.SetString("Email", uRegis.Email);

                string resetLink = Url.Action("Reset", "ResetPassword", new { token = token, email = uRegis.Email }, protocol: HttpContext.Request.Scheme);
                var response = await _session.SendEmailToUserActionAsync(uRegis.Email, "Name", "Reset your password", $"Please reset your password by clicking on this link: {resetLink}");

                return Json(new { success = response.Status });
            }
            return View(uRegis);
        }

        public IActionResult Reset(string token, string email)
        {
            if (!HttpContext.Session.TryGetValue("ResetToken", out byte[] storedTokenBytes) ||
                !HttpContext.Session.TryGetValue("Email", out byte[] storedEmailBytes) ||
                !HttpContext.Session.TryGetValue("ResetTokenExpiration", out byte[] storedExpirationBytes))
            {
                return RedirectToAction("Index", "ResetPassword");
            }

            string storedToken = System.Text.Encoding.UTF8.GetString(storedTokenBytes);
            string storedEmail = System.Text.Encoding.UTF8.GetString(storedEmailBytes);
            DateTime.TryParse(System.Text.Encoding.UTF8.GetString(storedExpirationBytes), out DateTime storedExpiration);

            if (storedToken != token || storedEmail != email || storedExpiration <= DateTime.Now)
            {
                return RedirectToAction("Index", "ResetPassword");
            }

            HttpContext.Session.SetString("Email", email);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResettingProcess(UserRegister data)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Index", "ResetPassword");
            }

            GeneralResp resp = await _session.ResetPasswordActionAsync(HttpContext.Session.GetString("Email"), data.Password);
            HttpContext.Session.Clear();

            return Json(new { success = resp.Status });
        }
    }
}
