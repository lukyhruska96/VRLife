using VrLifeAPI.Common;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API.APIs;
using VrLifeAPI.Provider.Core.Applications.DefaultApps;

namespace VrLifeAPI.Provider.API
{
    /// <summary>
    /// Interface OpenAPI na straně Providera.
    /// </summary>
    public interface IOpenAPI
    {
        /// <summary>
        /// Instance síťovacího serveru.
        /// </summary>
        IUDPNetworking<MainMessage> Networking { get; }

        /// <summary>
        /// Instance načteného konfiguračního souboru.
        /// </summary>
        IConfig Config { get; }

        /// <summary>
        /// Instance UserAPI na straně Providera.
        /// </summary>
        IUserAPI User { get;}

        /// <summary>
        /// Instance poskytovatele výchozích aplikací na straně Providera.
        /// </summary>
        IDefaultAppsProvider Apps { get; }

        /// <summary>
        /// Inicializace API.
        /// </summary>
        /// <param name="api">Instance ClosedAPI.</param>
        void Init(IClosedAPI api);

        /// <summary>
        /// Vytvoření loggeru pro danou třídu.
        /// </summary>
        /// <param name="className">Název třídy.</param>
        /// <returns>Instance loggeru pro danou třídu.</returns>
        ILogger CreateLogger(string className);

        /// <summary>
        /// Žádost o získání ClosedAPI, v případě
        /// privilegovaných aplikací.
        /// </summary>
        /// <param name="app">AppInfo objekt žádající aplikace.</param>
        /// <returns>Instance ClosedAPI v případě dostečného oprávnění.</returns>
        IClosedAPI GetClosedAPI(AppInfo app);
    }
}
