using Forums.Domain.Entities.User;

namespace Forums.Domain.Entities.Posts
{
    public class SavedPost
    {
        public int UserId { get; set; }
        public UDbTable User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }

}
