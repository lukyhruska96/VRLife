using System;

namespace VrLifeAPI.Client.Core.Services
{
    public interface ISystemServiceClient : IServiceClient
    {
        event Action ForwarderLost;

        event Action ProviderLost;

        void OnForwarderLost();

        void OnProviderLost();
    }
}
