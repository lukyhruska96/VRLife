using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;

namespace VrLifeServer.Core.Services
{
    interface IServiceProvider : IService
    {
        void Init(ClosedAPI api);
    }
}
