using Forums.BusinessLogic;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Posts;
using Forums.Domain.Entities.Response;
using Forums.Web.Extension;
using Forums.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Forums.Web.Controllers
{

    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly BusinessLogic.Interfaces.ISession _session;

        public PostController(IPost postService, BusinessLogic.Interfaces.ISession session)
                {
                    _postService = postService;
                    _session = session;
                }

        public IActionResult Index()
        {
            var loginStatus = HttpContext.Session.GetString("LoginStatus");
            ViewBag.IsAuthenticated = loginStatus == "login";
            if (ViewBag.IsAuthenticated == true)
            {
                return View();
            }
            else
            {
               return RedirectToAction("Index", "Login"); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(PostData postData)
        {
            if (ModelState.IsValid)
            {
                var user = HttpContext.GetMySessionObject();
                if (user != null)
                {
                    var postD = new Post
                    {
                        Content = postData.Content,
                        Title = postData.Title,
                        AuthorId = user.Id
                    };

                    GeneralResp generalResp = await _postService.SavePost(postD);

                    if (generalResp.Status)
                    {
                        return RedirectToAction("HomePage", "Home"); // Redirect to another action or view upon success
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, generalResp.StatusMsg); // Add error message to model state
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Login"); // Redirect to login if user is not authenticated
                }
            }

            return View(postData); // Return the view with the model if invalid
        }

        /*        [HttpPost]
                public async Task<IActionResult> SavePost()
                {
                    var user = HttpContext.GetMySessionObject();
                    var result = await _postService.SavePost(0, user.Id);

                    if (result.Status)
                    {
                        return Ok(new { Message = "Post saved successfully" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed to save post" });
                    }
                }*/
    }
}
