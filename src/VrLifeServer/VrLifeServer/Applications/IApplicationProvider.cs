using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;

namespace VrLifeServer.Applications
{
    interface IApplicationProvider : IApplicationServer
    {
        void Init(OpenAPI api);
    }
}
