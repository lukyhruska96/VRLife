using Assets.Scripts.API.ObjectAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace Assets.Scripts.Core.Applications.ObjectApp
{
    interface IObjectApp : IApplication
    {
        void Init(OpenAPI api, ObjectAPI objectAPI);
    }
}
