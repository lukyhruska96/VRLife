using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public class RedirectMsgHandler : IRedirectMsgHandler
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
                byte[] byteAddress = BitConverter.GetBytes(redirectMsg.Address);
                IPAddress address = new IPAddress(byteAddress);
                _networking.SendAsync(toSend, new IPEndPoint(address, redirectMsg.Port), 
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
