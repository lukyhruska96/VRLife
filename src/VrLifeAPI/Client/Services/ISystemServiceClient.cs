using System;

namespace VrLifeAPI.Client.Services
{
    public interface ISystemServiceClient : IServiceClient
    {
        event Action ForwarderLost;

        event Action ProviderLost;

        void OnForwarderLost();

        void OnProviderLost();
    }
}
