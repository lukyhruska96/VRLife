using System;
using System.Collections.Generic;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public class ClientIdFiller : IMiddleware<MainMessage>
    {
        ulong _id;
        public ClientIdFiller()
        {
            this._id = 0;
        }

        public ClientIdFiller(ulong id)
        {
            this._id = id;
        }

        public void SetId(ulong id)
        {
            this._id = id;
        }

        public MainMessage TransformInputMsg(MainMessage msg)
        {
            return msg;
        }

        public MainMessage TransformOutputMsg(MainMessage msg)
        {
            msg.ClientId = this._id;
            return msg;
        }
    }
}
