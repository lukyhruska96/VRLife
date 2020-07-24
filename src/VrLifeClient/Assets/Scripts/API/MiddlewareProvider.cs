using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeShared.Networking.Middlewares;

namespace Assets.Scripts.API
{
    class MiddlewareProvider : IMiddlewareProvider
    {
        private ClientIdFiller _clientIdFiller;
        public IClientIdFiller ClientIdFiller { get => _clientIdFiller; }

        private MsgIdIncrement _msgIdIncrement;
        public IMsgIdIncrement MsgIdIncrement { get => _msgIdIncrement; }

        private RedirectMsgHandler _redirectMsgHandler;
        public IRedirectMsgHandler RedirectMsgHandler { get => _redirectMsgHandler; }

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
