using Forums.Domain.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forums.BusinessLogic.Interfaces
{
    public interface IPost
    {
        Task<GeneralResp> SavePost(int userId, int postId);
        Task<GeneralResp> UnSavePost(int userId, int postId);
        Task<GeneralResp> ReplyToComment(int parentCommentId, int userId, string content);
    }
}
