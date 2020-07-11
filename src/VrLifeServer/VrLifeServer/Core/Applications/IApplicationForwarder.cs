using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;

namespace VrLifeServer.Applications
{
    interface IApplicationForwarder : IApplicationServer
    {
        void Init(OpenAPI api);
    }
}
