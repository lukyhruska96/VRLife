using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Forwarder.API;

namespace VrLifeAPI.Forwarder.Core.Services
{
    public interface IServiceForwarder : IService
    {
        void Init(IClosedAPI api);
    }
}
