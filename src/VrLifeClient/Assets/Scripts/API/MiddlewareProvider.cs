using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.API
{
    class MiddlewareProvider
    {
        private ClientIdFiller _clientIdFiller;
        public ClientIdFiller ClientIdFiller { get; }

        private MsgIdIncrement _msgIdIncrement;
        public MsgIdIncrement MsgIdIncrement { get; }

        private RedirectMsgHandler _redirectMsgHandler;
        public RedirectMsgHandler RedirectMsgHandler { get; }

        public MiddlewareProvider(
            ClientIdFiller clientIdFiller,
            MsgIdIncrement msgIdIncrement,
            RedirectMsgHandler redirectMsgHandler)
        {
            this._clientIdFiller = clientIdFiller;
            this._msgIdIncrement = msgIdIncrement;
            this._redirectMsgHandler = redirectMsgHandler;
        }

        public List<IMiddleware<MainMessage>> ToList()
        {
            var list = new List<IMiddleware<MainMessage>>();
            list.Add(_clientIdFiller);
            list.Add(_msgIdIncrement);
            list.Add(_redirectMsgHandler);
            return list;
        }
    }
}
