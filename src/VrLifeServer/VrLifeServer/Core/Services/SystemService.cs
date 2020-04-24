using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    class SystemService : IService
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            if(!VrLifeServer.Conf.IsMain)
            {
                InitStats();
            }
        }

        private void InitStats()
        {
                
        }

        public static MainMessage CreateHelloMessage()
        {
            MainMessage mainMsg = new MainMessage();
            SystemMsg msg = new SystemMsg();
            HiMsg hiMsg = new HiMsg();
            hiMsg.Memory = HwMonitor.GetTotalMemory();
            hiMsg.Threads = HwMonitor.GetCoreCount();
            msg.HiMsg = hiMsg;
            msg.Type = SystemMsgType.HiMsg;
            mainMsg.SystemMsg = msg;
            mainMsg.MsgType = MessageType.SystemMsg;
            return mainMsg;
        }
    }
}
