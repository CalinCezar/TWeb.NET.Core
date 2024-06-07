using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using System.Threading.Tasks;

namespace Forums.BusinessLogic.Interfaces
{
    public interface IUser
    {
        Task<UserMinimal> GetUserDataActionAsync(int ID);
        Task<GeneralResp> DeleteUserActionAsync(int ID);
        Task<GeneralResp> EditUserDataActionAsync(UserMinimal data, int ID);
        Task<GeneralResp> UploadPhotoActionAsync(string photo, int ID);
    }
}
