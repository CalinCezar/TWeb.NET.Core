﻿using Forums.Domain.Entities.User;

namespace Forums.Domain.Entities.Posts
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public UDbTable User { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
    }
}
