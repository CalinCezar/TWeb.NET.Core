using Forums.BusinessLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Forums.BusinessLogic
{
    public class BusinessLogic
    {
        private readonly IServiceProvider _serviceProvider;

        public BusinessLogic(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISession GetSessionBL()
        {
            return _serviceProvider.GetRequiredService<ISession>();
        }

        public IUser GetUserBL()
        {
            return _serviceProvider.GetRequiredService<IUser>();
        }

        public IPost GetPostBL()
        {
            return _serviceProvider.GetRequiredService<IPost>();
        }
    }
}
