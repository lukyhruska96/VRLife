using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services
{
    public interface IService
    {
        MainMessage HandleMessage(MainMessage msg);
    }
}
