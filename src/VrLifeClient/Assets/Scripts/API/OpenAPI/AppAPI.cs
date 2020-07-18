using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeClient.Core.Services.AppService;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.API.OpenAPI
{
    class AppAPI
    {
        private AppServiceClient _service;
        public AppAPI(AppServiceClient service)
        {
            _service = service;
        }

        public ServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient)
        {
            return _service.SendAppMsg(app, data, recipient);
        }
    }
}
