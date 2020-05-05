using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeShared.Networking;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient
{
    class Init : MonoBehaviour
    {
        private UDPNetworking<MainMessage> _listener;
        private List<IMiddleware<MainMessage>> _middlewares = new List<IMiddleware<MainMessage>>();
        private void Awake()
        {
            _middlewares.Add(new ClientIdFiller());
            _middlewares.Add(new MsgIdIncrement());
            _middlewares.Add(new RedirectMsgHandler());
            _listener = new UDPNetworking<MainMessage>(_middlewares);
        }
    }
}
