using Forums.BusinessLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
