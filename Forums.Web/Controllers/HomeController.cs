using Forums.Web.Extension;
using Forums.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forums.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }
    }
}
