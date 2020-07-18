using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services.UserService;

namespace VrLifeServer.API.Provider.APIs
{
    class UserAPI
    {
        private ClosedAPI _api;
        public UserAPI(ClosedAPI api)
        {
            this._api = api;
        }

        public ulong? ClientId2UserId(ulong clientId)
        {
            User u = _api.Services.User.GetUserByClientId(clientId);
            if(u == null)
            {
                return null;
            }
            return u.Id;
        }
    }
}
