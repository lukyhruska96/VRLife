using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.UserService
{
    interface IUserService : IService
    {
        ulong GetUserId(uint clientId);
    }
}
