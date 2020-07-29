using Google.Protobuf;

namespace VrLifeAPI.Networking
{

    /// <summary>
    /// Interface UDP síťovacího serveru.
    /// </summary>
    /// <typeparam name="T">Typ přijímaných a odesílaných dat.</typeparam>
    public interface IUDPNetworking<T> : INetworking<T> where T: IMessage<T>, new()
    {

    }
}
