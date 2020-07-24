using System;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface ISystemAPI
    {
        event Action ForwarderLost;
        event Action ProviderLost;
    }
}
