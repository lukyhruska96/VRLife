using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VrLifeServer.Core.Utils;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    struct ObjectState
    {
        public ulong ObjectInstanceId { get; set; }
        public Vector3 Center { get; set; }
        public byte[] ObjectData { get; set; }

        public GameObject ToNetworkModel()
        {
            GameObject o = new GameObject();
            o.ObjectInstanceId = ObjectInstanceId;
            o.Center = Center.ToCoord();
            o.ObjectData = ByteString.CopyFrom(ObjectData);
            return o;
        }

    }
}
