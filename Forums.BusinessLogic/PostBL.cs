using System.Threading.Tasks;
using AutoMapper;
using Forums.BusinessLogic.Core;
using Forums.BusinessLogic.DBModel;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Posts;
using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using Microsoft.AspNetCore.Http;

namespace Forums.BusinessLogic
{
    public class PostBL : UserApi, IPost
    {
        public PostBL(UserContext userContext, SessionContext sessionContext, IMapper mapper, PostContext postContext) :
            base(userContext, sessionContext, mapper,postContext)
        {
        }
        public Task<GeneralResp> SavePost(int userId, int postId)
        {
            return SavePostAsync(userId, postId);
        }
        public Task<GeneralResp> UnSavePost(int userId, int postId)
        {
            return UnSavePostAsync(userId, postId);
        }
        public Task<GeneralResp> ReplyToComment(int parentCommentId, int userId, string content)
        {
            return ReplyToCommentAsync(parentCommentId, userId, content);
        }
    }
}
