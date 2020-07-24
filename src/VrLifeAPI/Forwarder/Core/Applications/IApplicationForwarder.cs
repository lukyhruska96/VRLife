using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;

namespace VrLifeAPI.Forwarder.Core.Applications
{
    public interface IApplicationForwarder : IApplicationServer
    {
        void Init(uint roomId, IOpenAPI api, IAppDataStorage appStorage);
    }
}
