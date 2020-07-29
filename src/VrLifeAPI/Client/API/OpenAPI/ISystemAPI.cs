using System;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci SystemService pomocí OpenAPI
    /// </summary>
    public interface ISystemAPI
    {
        /// <summary>
        /// Event volaný v případě, že se nepodařilo
        /// již delší dobu spojit s Forwarderem
        /// </summary>
        event Action ForwarderLost;

        /// <summary>
        /// Event volaný v případě, že se nepodařilo
        /// již delší dobu spojit s Providerem
        /// </summary>
        event Action ProviderLost;
    }
}
