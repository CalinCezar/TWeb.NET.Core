using System.Threading.Tasks;
using AutoMapper;
using Forums.BusinessLogic.Core;
using Forums.BusinessLogic.DBModel;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using Microsoft.AspNetCore.Http;

namespace Forums.BusinessLogic
{
    public class SessionBL : UserApi, Forums.BusinessLogic.Interfaces.ISession
    {
        public SessionBL(UserContext userContext, SessionContext sessionContext, IMapper mapper)
            : base(userContext, sessionContext, mapper)
        {
        }

        public Task<GeneralResp> UserPassCheckActionAsync(ULoginData data)
        {
            return UserAuthLogicAsync(data);
        }

        public Task<GeneralResp> RegisterNewUserActionAsync(URegisterData data)
        {
            return RegisterUserActionAsync(data);
        }

        public Task<string> GenCookieAsync(string loginCredential)
        {
            return CreateCookieAsync(loginCredential);
        }

        public Task<UserMinimal> GetUserByCookieAsync(string apiCookieValue)
        {
            return UserCookieAsync(apiCookieValue);
        }

        public Task<GeneralResp> SendEmailToUserActionAsync(string email, string name, string subject, string body)
        {
            return SendEmailAsync(email, name, subject, body);
        }

        public Task<GeneralResp> ResetPasswordActionAsync(string email, string password)
        {
            return ResetPasswordAsync(email, password);
        }

        public Task<GeneralResp> ExistingEmailInDBAsync(string email)
        {
            return ExistingEmailAsync(email);
        }
    }
}
