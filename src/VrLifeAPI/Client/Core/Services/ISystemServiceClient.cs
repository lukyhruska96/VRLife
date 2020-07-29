using System;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Interface systémové služby.
    /// </summary>
    public interface ISystemServiceClient : IServiceClient
    {
        /// <summary>
        /// Událost vyvolaná v případě, že bylo ztraceno spojení s forwarderem.
        /// </summary>
        event Action ForwarderLost;

        /// <summary>
        /// Událost vyvolaná v případě, že bylo ztraceno spojení s providerem.
        /// </summary>
        event Action ProviderLost;

        /// <summary>
        /// Vyvolání události ForwarderLost.
        /// </summary>
        void OnForwarderLost();

        /// <summary>
        /// Vyvolání události ProviderLost.
        /// </summary>
        void OnProviderLost();
    }
}
