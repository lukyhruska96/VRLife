using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeClient.API.GlobalAPI;
using VrLifeClient.API.OpenAPI;

namespace Assets.Scripts.Core.Applications.GlobalApp
{
    interface IGlobalApp : IApplication
    {
        void Init(OpenAPI api, GlobalAPI globalAPI);
    }
}
