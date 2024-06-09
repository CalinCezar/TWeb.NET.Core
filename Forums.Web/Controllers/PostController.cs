using Forums.Web.Filters;
using Microsoft.AspNetCore.Mvc;
namespace Forums.Web.Controllers
{
    
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
