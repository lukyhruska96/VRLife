using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Applications.DefaultApps;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API
{
    /// <summary>
    /// API přístupné pro všechny objekty.
    /// </summary>
    public interface IOpenAPI
    {
        /// <summary>
        /// Instance IUDPNetworking.
        /// </summary>
        IUDPNetworking<MainMessage> Networking { get; }

        /// <summary>
        /// Instance lokální konfigurace.
        /// </summary>
        IConfig Config { get; }

        /// <summary>
        /// Instance UserAPI.
        /// </summary>
        IUserAPI User { get; }

        /// <summary>
        /// Instance RoomAPI.
        /// </summary>
        IRoomAPI Room { get; }

        /// <summary>
        /// Instance EventAPI.
        /// </summary>
        IEventAPI Event { get; }

        /// <summary>
        /// Instance TickAPI.
        /// </summary>
        ITickAPI TickRate { get; }

        /// <summary>
        /// Instance SystemAPI.
        /// </summary>
        ISystemAPI System { get; }

        /// <summary>
        /// Instance providera výchozích aplikací.
        /// </summary>
        IDefaultApps DefaultApps { get; }

        /// <summary>
        /// Instance AppAPI.
        /// </summary>
        IAppAPI App { get; }

        /// <summary>
        /// Getter ClosedAPI, kdy dotazovaná aplikace musí mít dostatčená privilegia.
        /// </summary>
        /// <param name="info">AppInfo dané aplikace.</param>
        /// <returns>Instance ClosedAPI, Exception v případně nedostatečných privilegií.</returns>
        IClosedAPI GetClosedAPI(AppInfo info);

        /// <summary>
        /// Vytvoření loggeru pro danou třídu.
        /// </summary>
        /// <param name="className">Název třídy zobrazený jako vedlejší informace každého záznamu z vrácené instance.</param>
        /// <returns>Instance loggeru.</returns>
        ILogger CreateLogger(string className);
    }
}
