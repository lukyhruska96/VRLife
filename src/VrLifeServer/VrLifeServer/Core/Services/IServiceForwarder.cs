using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;

namespace VrLifeServer.Core.Services
{
    interface IServiceForwarder : IService
    {
        void Init(ClosedAPI api);
    }
}
