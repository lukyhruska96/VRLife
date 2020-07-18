using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace VrLifeClient.API.ObjectAPI
{
    class ObjectAPI
    {
        private ClosedAPI _api;
        public ObjectAPI(ClosedAPI api)
        {
            this._api = api;
        }
    }
}
