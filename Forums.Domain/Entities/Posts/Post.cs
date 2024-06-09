namespace Forums.Domain.Entities.Posts
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<SavedPost> SavedByUsers { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
