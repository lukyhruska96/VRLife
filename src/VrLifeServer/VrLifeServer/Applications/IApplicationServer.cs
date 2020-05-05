using System;
using VrLifeServer.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Applications
{
    interface IApplicationServer
    {
        void Init(OpenAPI api);
        AppMsg HandleEvent(EventMsg eventMsg);
    }
}
