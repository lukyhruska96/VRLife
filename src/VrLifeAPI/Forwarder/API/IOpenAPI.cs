using VrLifeAPI.Common;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.Core.Applications.DefaultApps;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Forwarder.API
{
    /// <summary>
    /// Interface OpenAPI na straně Forwarder Serveru.
    /// </summary>
    public interface IOpenAPI
    {
        /// <summary>
        /// Instance síťového serveru.
        /// </summary>
        IUDPNetworking<MainMessage> Networking { get; }

        /// <summary>
        /// Instance načteného konfiguračního souboru.
        /// </summary>
        IConfig Config { get; }

        /// <summary>
        /// Instance poskytovatele výchozích aplikací na straně Forwardera.
        /// </summary>
        IDefaultAppsForwarder Apps { get; }

        /// <summary>
        /// Inicializace API
        /// </summary>
        /// <param name="api">Instance ClosedAPI</param>
        void Init(IClosedAPI api);

        /// <summary>
        /// Získání instance loggeru pro danou třídu.
        /// </summary>
        /// <param name="className">Název třídy.</param>
        /// <returns>Instance loggeru pro danou třídu.</returns>
        ILogger CreateLogger(string className);

        /// <summary>
        /// Getter pro získání ClosedAPI pro privilegované aplikace.
        /// </summary>
        /// <param name="app">AppInfo objekt dané aplikace.</param>
        /// <returns>Instance ClosedAPI v případě dostatečných práv.</returns>
        IClosedAPI GetClosedAPI(AppInfo app);
        
    }
}
