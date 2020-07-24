using Assets.Scripts.API.HUDAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.HUDAPI
{
    class HUDAPI : IHUDAPI
    {
        private ClosedAPI _api;

        public HUDAPI(ClosedAPI api)
        {
            this._api = api;
        }

        public void ShowNotification(Notification notification)
        {
            HUDController.current?.ShowNotification(notification);
        }
    }
}
