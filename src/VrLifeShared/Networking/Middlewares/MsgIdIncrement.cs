using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public class MsgIdIncrement : IMiddleware<MainMessage>
    {
        private long msgId = 0;
        public MainMessage TransformInputMsg(MainMessage msg)
        {
            return msg;
        }

        public MainMessage TransformOutputMsg(MainMessage msg)
        {
            msg.MsgId = (ulong)Interlocked.Increment(ref msgId) - 1;
            return msg;
        }
    }
}
