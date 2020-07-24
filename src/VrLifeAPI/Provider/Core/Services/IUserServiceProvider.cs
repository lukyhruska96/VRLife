using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.UserService;

namespace VrLifeAPI.Provider.Core.Services.UserService
{
    public interface IUserServiceProvider : IUserService, IServiceProvider
    {
        IUser GetUserByClientId(ulong clientId);
    }
}
