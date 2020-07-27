using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Common.Core.Services.SystemService;

namespace VrLifeAPI.Provider.Core.Services.SystemService
{
    public interface ISystemServiceProvider : ISystemService, IServiceProvider
    {
        bool IsAlive(ulong serverId);

        IPEndPoint GetAddressById(uint serverId);
    }
}
