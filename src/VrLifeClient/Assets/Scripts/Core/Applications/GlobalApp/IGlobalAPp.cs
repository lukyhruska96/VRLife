using Assets.Scripts.API.GlobalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace Assets.Scripts.Core.Applications.GlobalApp
{
    interface IGlobalApp : IApplication
    {
        void Init(OpenAPI api, GlobalAPI globalAPI);
    }
}
