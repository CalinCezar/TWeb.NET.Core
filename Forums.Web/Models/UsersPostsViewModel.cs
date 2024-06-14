using Forums.Domain.Entities.Posts;

namespace Forums.Web.Models
{
    public class UsersPostsViewModel: UserData
    {
        public List<Post> Posts { get; set; }
    }
}
