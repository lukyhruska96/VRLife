using Google.Protobuf;

namespace VrLifeAPI.Networking
{

    public interface IUDPNetworking<T> : INetworking<T> where T: IMessage<T>, new()
    {

    }
}
