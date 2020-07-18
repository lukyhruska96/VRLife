using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeClient.API.ObjectAPI;
using VrLifeClient.API.OpenAPI;

namespace Assets.Scripts.Core.Applications.ObjectApp
{
    interface IObjectApp : IApplication
    {
        void Init(OpenAPI api, ObjectAPI objectAPI);
    }
}
