using Forums.Domain.Entities.Posts;

namespace Forums.Web.Models
{
    public class UsersPostsViewModel
    {
        public UserData User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
