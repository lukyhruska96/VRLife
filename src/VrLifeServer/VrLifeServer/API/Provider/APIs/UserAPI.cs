using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.UserService;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.API.APIs;
using VrLifeServer.Core.Services.UserService;

namespace VrLifeServer.API.Provider.APIs
{
    class UserAPI : IUserAPI
    {
        private IClosedAPI _api;
        public UserAPI(IClosedAPI api)
        {
            this._api = api;
        }

        public ulong? ClientId2UserId(ulong clientId)
        {
            IUser u = _api.Services.User.GetUserByClientId(clientId);
            if(u == null)
            {
                return null;
            }
            return u.Id;
        }
    }
}
