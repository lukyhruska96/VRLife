using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.UserService
{
    interface IUserServiceForwarder : IUserService, IServiceForwarder
    {
        User GetUserById(ulong userId, bool cached = false);
        ulong? GetUserIdByClientId(ulong clientId);
        ulong? GetClientIdByUserId(ulong userId);
    }
}
