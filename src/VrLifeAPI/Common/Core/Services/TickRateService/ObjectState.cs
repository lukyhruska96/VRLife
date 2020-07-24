using Google.Protobuf;
using System.Numerics;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.TickRateService
{
    public struct ObjectState
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
