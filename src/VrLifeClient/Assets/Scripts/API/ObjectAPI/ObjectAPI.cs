using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeClient.API;

namespace VrLifeClient.API.ObjectAPI
{
    class ObjectAPI : IObjectAPI
    {
        private ClosedAPI _api;
        public ObjectAPI(ClosedAPI api)
        {
            this._api = api;
        }
    }
}
