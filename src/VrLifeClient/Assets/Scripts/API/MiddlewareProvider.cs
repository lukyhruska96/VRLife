using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.API
{
    class MiddlewareProvider
    {
        private ClientIdFiller _clientIdFiller;
        public ClientIdFiller ClientIdFiller { get => _clientIdFiller; }

        private MsgIdIncrement _msgIdIncrement;
        public MsgIdIncrement MsgIdIncrement { get => _msgIdIncrement; }

        private RedirectMsgHandler _redirectMsgHandler;
        public RedirectMsgHandler RedirectMsgHandler { get => _redirectMsgHandler; }

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
