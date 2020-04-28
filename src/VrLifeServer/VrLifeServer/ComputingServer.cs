using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services;
using VrLifeServer.Networking;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer
{
    class ComputingServer
    {

        private UDPNetworking<MainMessage> udpListenner;
        private SystemServiceProvider sysService = new SystemServiceProvider();
        private uint serverId = 0;

        public void Init()
        {
            udpListenner = new UDPNetworking<MainMessage>(VrLifeServer.Conf.Address, (int)VrLifeServer.Conf.UdpPort, this.MsgRouter);
        }

        public void Start()
        {
            // say hi
            udpListenner.Send(SystemServiceProvider.CreateHelloMessage(),
                VrLifeServer.Conf.MainServer,
                this.AfterFirstResponse);
        }

        private void AfterFirstResponse(MainMessage msg)
        {
            SystemMsg sysMsg = msg.SystemMsg;
            if(sysMsg.SystemMsgTypeCase.Equals(SystemMsg.SystemMsgTypeOneofCase.ErrorMsg))
            {
                throw new ServerException(sysMsg.ErrorMsg.ErrorMsg_);
            }
            serverId = msg.ServerId;
            sysService.Init();
            udpListenner.StartListening();
        }

        private MainMessage MsgRouter(MainMessage msg)
        {
            MainMessage response;
            switch(msg.SystemMsg.SystemMsgTypeCase)
            {
                default:
                    response = SystemServiceProvider.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown message type");
                    break;
            }
            response.ServerId = serverId;
            return response;
        }
    }
}
