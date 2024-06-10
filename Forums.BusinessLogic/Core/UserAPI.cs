using AutoMapper;
using Forums.BusinessLogic.DBModel;
using Forums.Domain.Entities.Response;
using Forums.Domain.Entities.User;
using Forums.Domain.Enum;
using Forums.Helpers;
using Forums.BusinessLogic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Forums.Domain.Entities.Posts;

namespace Forums.BusinessLogic.Core
{
    public class UserApi
    {
        private readonly UserContext _userContext;
        private readonly PostContext _postContext;
        private readonly SessionContext _sessionContext;
        private readonly IMapper _mapper;

        public UserApi(UserContext userContext, SessionContext sessionContext, IMapper mapper, PostContext postContext)
        {
            _postContext = postContext;
            _userContext = userContext;
            _sessionContext = sessionContext;
            _mapper = mapper;
        }

        public async Task<GeneralResp> UserAuthLogicAsync(ULoginData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Credential))
            {
                var pass = LoginHelper.HashGen(data.Password);
                result = await _userContext.Users
                    .FirstOrDefaultAsync(u => (u.Username == data.Credential || u.Email == data.Credential) && u.Password == pass);

                if (result == null)
                {
                    return new GeneralResp { Status = false, StatusMsg = "The Username or Password is Incorrect" };
                }

                result.LasIP = data.LoginIP;
                result.LastLogin = data.LoginDateTime;
                _userContext.Users.Update(result);
                await _userContext.SaveChangesAsync();

                return new GeneralResp { Status = true };
            }
            else
            {
                var pass = LoginHelper.HashGen(data.Password);
                result = await _userContext.Users
                    .FirstOrDefaultAsync(u => u.Username == data.Credential && u.Password == pass);

                if (result == null)
                {
                    return new GeneralResp { Status = false, StatusMsg = "The Username or Password is Incorrect" };
                }

                result.LasIP = data.LoginIP;
                result.LastLogin = data.LoginDateTime;
                _userContext.Users.Update(result);
                await _userContext.SaveChangesAsync();

                return new GeneralResp { Status = true };
            }
        }

        public async Task<GeneralResp> RegisterUserActionAsync(URegisterData data)
        {
            var newUser = new UDbTable
            {
                Username = data.Credential,
                Password = LoginHelper.HashGen(data.Password),
                Email = data.Email,
                InfoBlog = data.InfoBlog,
                LastLogin = data.LoginDateTime,
                LasIP = data.LoginIp,
                Level = UserRole.User,
                Profession = string.Empty,
                PhoneNumber = string.Empty,
                Photo = string.Empty,
                Fullname = string.Empty
            };

            if (await _userContext.Users.AnyAsync(u => u.Email == data.Email))
            {
                return new GeneralResp { Status = false, StatusMsg = "Email already exists" };
            }

            _userContext.Users.Add(newUser);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true };
        }

        public async Task<string> CreateCookieAsync(string loginCredential)
        {
            var apiCookie = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(60).UtcDateTime // Convert DateTimeOffset to DateTime
            };

            var validate = new EmailAddressAttribute();
            var curent = validate.IsValid(loginCredential)
                ? await _sessionContext.Sessions.FirstOrDefaultAsync(e => e.Username == loginCredential)
                : await _sessionContext.Sessions.FirstOrDefaultAsync(e => e.Username == loginCredential);

            var cookieValue = CookieGenerator.Create(loginCredential);

            if (curent != null)
            {
                curent.CookieString = cookieValue;
                curent.ExpireTime = DateTime.Now.AddMinutes(60);
                _sessionContext.Sessions.Update(curent);
            }
            else
            {
                await _sessionContext.Sessions.AddAsync(new Session
                {
                    Username = loginCredential,
                    CookieString = cookieValue,
                    ExpireTime = DateTime.Now.AddMinutes(60)
                });
            }

            await _sessionContext.SaveChangesAsync();
            return cookieValue;
        }

        public async Task<UserMinimal> UserCookieAsync(string cookie)
        {
            var session = await _sessionContext.Sessions.FirstOrDefaultAsync(s => s.CookieString == cookie && s.ExpireTime > DateTimeOffset.Now);
            if (session == null) return null;

            var curentUser = await _userContext.Users.FirstOrDefaultAsync(u => u.Email == session.Username || u.Username == session.Username);
            if (curentUser == null) return null;

            return _mapper.Map<UserMinimal>(curentUser);
        }

        public async Task<UserMinimal> GetUserDataAsync(int ID)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Id == ID);
            if (result == null) return null;

            return _mapper.Map<UserMinimal>(result);
        }

        public async Task<GeneralResp> UploadPhotoAsync(string photo, int ID)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Id == ID);
            if (result == null || result.Photo == photo) return new GeneralResp { Status = false };

            result.Photo = photo;
            _userContext.Users.Update(result);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true };
        }

        public async Task<GeneralResp> EditUserDataAsync(UserMinimal data, int ID)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Id == ID);
            if (result == null) return new GeneralResp { Status = false };

            if (result.Fullname == data.Fullname && result.Email == data.Email && result.InfoBlog == data.InfoBlog &&
                result.PhoneNumber == data.PhoneNumber && result.Profession == data.Profession)
            {
                return new GeneralResp { Status = false };
            }

            result.Fullname = data.Fullname;
            result.Email = data.Email;
            result.InfoBlog = data.InfoBlog;
            result.PhoneNumber = data.PhoneNumber;
            result.Profession = data.Profession;

            _userContext.Users.Update(result);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true };
        }

        public async Task<GeneralResp> DeleteUserAsync(int ID)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Id == ID);
            if (result == null) return new GeneralResp { Status = false };

            _userContext.Users.Remove(result);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true };
        }

        public async Task<GeneralResp> SendEmailAsync(string email, string name, string subject, string body)
        {
            // Ensure that EmailService has a method called SendEmailAsync
            return new GeneralResp { Status = await EmailService.SendEmailToUserAsync(email, name, subject, body) };
        }

        public async Task<GeneralResp> ResetPasswordAsync(string email, string password)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (result == null) return new GeneralResp { Status = false };

            result.Password = LoginHelper.HashGen(password);
            _userContext.Users.Update(result);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true };
        }

        public async Task<GeneralResp> ExistingEmailAsync(string email)
        {
            var result = await _userContext.Users.FirstOrDefaultAsync(e => e.Email == email);
            return result == null ? new GeneralResp { Status = false } : new GeneralResp { Status = true };
        }

        public async Task<GeneralResp> SavePostAsync(Post postData)
        {
            if (postData == null)
            {
                return new GeneralResp { Status = false, StatusMsg = "Post data cannot be null" };
            }
           
            _postContext.Posts.Add(postData);
            await _postContext.SaveChangesAsync();

            return new GeneralResp { Status = true, StatusMsg = "Post saved successfully" };
        }


        /*public async Task<GeneralResp> UnSavePostAsync(int userId, int postId)
        {
            var savedPost = await _postContext.SavedPosts
                .FirstOrDefaultAsync(sp => sp.UserId == userId && sp.PostId == postId);

            if (savedPost == null)
            {
                return new GeneralResp { Status = false, StatusMsg = "Post not saved" };
            }

            _postContext.SavedPosts.Remove(savedPost);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true, StatusMsg = "Post unsaved successfully" };
        }

        public async Task<GeneralResp> ReplyToCommentAsync(int parentCommentId, int userId, string content)
        {
            var parentComment = await _postContext.Comments.FindAsync(parentCommentId);

            if (parentComment == null)
            {
                return new GeneralResp { Status = false, StatusMsg = "Parent comment not found" };
            }

            var reply = new Comment
            {
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                ParentCommentId = parentCommentId,
                PostId = parentComment.PostId
            };

            _postContext.Comments.Add(reply);
            await _userContext.SaveChangesAsync();

            return new GeneralResp { Status = true, StatusMsg = "Reply added successfully" };
        }*/

    }
}
