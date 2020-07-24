using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.Core.Utils;

namespace VrLifeServer.Core.Services
{
    static class ServiceUtils
    {
        public static MainMessage CreateHelloMessage(IConfig conf)
        {
            MainMessage mainMsg = new MainMessage();
            SystemMsg msg = new SystemMsg();
            HiMsg hiMsg = new HiMsg();
            hiMsg.Address = conf.ServerAddress.ToInt();
            hiMsg.Port = (int)conf.UdpPort;
            hiMsg.Memory = HwMonitor.GetTotalMemory();
            hiMsg.Threads = HwMonitor.GetCoreCount();
            hiMsg.Version = VrLifeServer.VERSION;
            msg.HiMsg = hiMsg;
            mainMsg.SystemMsg = msg;
            return mainMsg;
        }
    }
}
