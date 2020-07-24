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
    public interface IMiddlewareProvider
    {
        IClientIdFiller ClientIdFiller { get; }
        IMsgIdIncrement MsgIdIncrement { get; }
        IRedirectMsgHandler RedirectMsgHandler { get; }

        List<IMiddleware<MainMessage>> ToList();
    }
}
