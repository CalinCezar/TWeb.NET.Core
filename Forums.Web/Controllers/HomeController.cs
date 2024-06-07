using Forums.Web.Extension;
using Forums.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forums.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly BusinessLogic.Interfaces.ISession _session;

        public HomeController(BusinessLogic.Interfaces.ISession session) : base(session)
        {
            _session = session;
        }
        [ServiceFilter(typeof(AdminActionFilter))]
        [ServiceFilter(typeof(AuthorisedActionFilter))]
        public IActionResult HomePage()
        {
            SessionStatus();
            return View();
        }
    }
}
