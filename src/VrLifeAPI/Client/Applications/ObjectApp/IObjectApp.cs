using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeClient.API;

namespace VrLifeAPI.Client.Applications.ObjectApp
{
    public interface IObjectApp : IApplication
    {
        void Init(IOpenAPI api, IObjectAPI objectAPI);
    }
}
