using Forums.BusinessLogic;
using Forums.BusinessLogic.DBModel;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Posts;
using Forums.Domain.Entities.Response;
using Forums.Web.Extension;
using Forums.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Forums.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostContext _postContext;
        private readonly UserContext _userContext;

        public HomeController(PostContext postContext, UserContext userContext)
        {
            _postContext = postContext;
            _userContext = userContext;
        }

        public async Task<IActionResult> HomePage()
        {
            var posts = await _postContext.Posts.Take(10).ToListAsync();

            var postDataList = new List<PostUserViewModel>();

            foreach (var post in posts)
            {
                var user = await _userContext.Users.FindAsync(post.AuthorId);
                var userData = new UserData
                {
                    Username = user.Username,
                    Photo = user.Photo,
                };

                var postData = new PostUserViewModel
                {
                    post = post,
                    User = userData,
                };

                postDataList.Add(postData);
            }

            // Pass the list of view models to the view
            return View(postDataList);
        }
    }
}
