using System.Threading.Tasks;
using AutoMapper;
using Forums.BusinessLogic.Core;
using Forums.BusinessLogic.DBModel;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;

namespace Forums.BusinessLogic
{
    public class UserBL : UserApi, IUser
    {
        public UserBL(UserContext userContext, SessionContext sessionContext, IMapper mapper, PostContext postContext)
            : base(userContext, sessionContext, mapper, postContext)
        {
        }

        public Task<UserMinimal> GetUserDataActionAsync(int ID)
        {
            return GetUserDataAsync(ID);
        }

        public Task<GeneralResp> EditUserDataActionAsync(UserMinimal data, int ID)
        {
            return EditUserDataAsync(data, ID);
        }

        public Task<GeneralResp> DeleteUserActionAsync(int ID)
        {
            return DeleteUserAsync(ID);
        }

        public Task<GeneralResp> UploadPhotoActionAsync(string photo, int ID)
        {
            return UploadPhotoAsync(photo, ID);
        }
    }
}
