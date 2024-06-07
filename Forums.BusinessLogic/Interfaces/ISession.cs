using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Forums.BusinessLogic.Interfaces
{
    public interface ISession
    {
        Task<GeneralResp> UserPassCheckActionAsync(ULoginData data);
        Task<GeneralResp> RegisterNewUserActionAsync(URegisterData data);
        Task<string> GenCookieAsync(string loginCredential);
        Task<UserMinimal> GetUserByCookieAsync(string apiCookieValue);
        Task<GeneralResp> SendEmailToUserActionAsync(string email, string name, string subject, string body);
        Task<GeneralResp> ResetPasswordActionAsync(string email, string password);
        Task<GeneralResp> ExistingEmailInDBAsync(string email);
    }
}
