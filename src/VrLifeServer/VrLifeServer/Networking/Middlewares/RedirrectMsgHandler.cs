using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Networking.Middlewares
{
    class RedirrectMsgHandler : IMiddleware<MainMessage>
    {
        private INetworking<MainMessage> _networking = null;
        public void SetListenner(INetworking<MainMessage> networking)
        {
            this._networking = networking;
        }
        public MainMessage TransformInputMsg(MainMessage msg)
        {
            if(msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg 
                && msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.RedirectMsg)
            {
                if(_networking == null)
                {
                    return msg;
                }
                var t = new TaskCompletionSource<MainMessage>();
                RedirectMsg redirectMsg = msg.SystemMsg.RedirectMsg;
                MainMessage toSend = MainMessage.Parser.ParseFrom(redirectMsg.ReceivedMsg.ToByteArray());
                _networking.Send(toSend, new IPEndPoint(redirectMsg.Address, redirectMsg.Port), 
                    (x) => t.SetResult(x), (x) => t.SetResult(msg));
                t.Task.Wait();
                return t.Task.Result;
            }
            return msg;
        }

        public MainMessage TransformOutputMsg(MainMessage msg)
        {
            return msg;
        }
    }
}
