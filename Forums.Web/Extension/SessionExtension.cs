using Forums.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Forums.Web.Extension
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public static UserMinimal GetMySessionObject(this HttpContext current)
        {
            var sessionData = current?.Session.GetString("__SessionObject");
            return sessionData == null ? null : JsonConvert.DeserializeObject<UserMinimal>(sessionData);
        }

        public static void SetMySessionObject(this HttpContext current, UserMinimal profile)
        {
            var sessionData = JsonConvert.SerializeObject(profile);
            current.Session.SetString("__SessionObject", sessionData);
        }
    }
}
