using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.Posts;

namespace Forums.BusinessLogic.Interfaces
{
    public interface IPost
    {
        Task<GeneralResp> SavePost(Post postData);
/*        Task<GeneralResp> UnSavePost(int userId, int postId);
        Task<GeneralResp> ReplyToComment(int parentCommentId, int userId, string content);*/
    }
}
