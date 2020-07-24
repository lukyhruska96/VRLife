using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeClient.API;

namespace VrLifeClient.API.GlobalAPI
{
    class GlobalAPI : IGlobalAPI
    {
        private ClosedAPI _api;

        public IPlayersAPI Players { get; private set; }


        public GlobalAPI(ClosedAPI api)
        {
            this._api = api;
            Players = new PlayersAPI(_api);
        }
    }
}
