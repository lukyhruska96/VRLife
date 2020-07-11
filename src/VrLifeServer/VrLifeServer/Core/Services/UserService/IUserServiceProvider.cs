using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.UserService
{
    interface IUserServiceProvider : IUserService, IServiceProvider
    {
        User GetUserByClientId(ulong clientId);
    }
}
