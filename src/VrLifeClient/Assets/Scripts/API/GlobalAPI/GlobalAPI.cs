using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace Assets.Scripts.API.GlobalAPI
{
    class GlobalAPI
    {
        private ClosedAPI _api;
        public GlobalAPI(ClosedAPI api)
        {
            this._api = api;
        }
    }
}
