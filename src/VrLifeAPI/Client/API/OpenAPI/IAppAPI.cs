using System.Collections.Generic;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Services;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci s AppService pomocí OpenAPI.
    /// </summary>
    public interface IAppAPI
    {
        /// <summary>
        /// Odeslání zprávy adresátovi dle volby recipient.
        /// </summary>
        /// <param name="app">AppInfo dané aplikace.</param>
        /// <param name="data">Data k odeslání ve formě byte array.</param>
        /// <param name="recipient">Adresát dané zprávy (Provider/Forwarder)</param>
        /// <returns>ServiceCallback s návratovou hodnotou byte array.</returns>
        IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient);

        /// <summary>
        /// Seznam všech lokálně zinicializovaných menu aplikací.
        /// </summary>
        List<IMenuApp> MenuApps { get; }
        /// <summary>
        /// Seznam všech lokálně zinicializovaných globálních aplikací.
        /// </summary>
        List<IGlobalAPp> GlobalApps { get; }
        /// <summary>
        /// Seznam všech lokálně zinicializovaných aplikací v pozadí.
        /// </summary>
        List<IBackgroundApp> BackgroundApps { get; }

        /// <summary>
        /// Seznam všech lokálně zinicializovaných objektových aplikací.
        /// </summary>
        List<IObjectApp> ObjectApps { get; }

        /// <summary>
        /// Slovník seznamů všech instancí k dané objektové aplikaci podle jejího ID.
        /// </summary>
        Dictionary<ulong, List<IObjectAppInstance>> ObjectAppInstances { get; }

        /// <summary>
        /// Slovník všech lokálně zinicializovaných aplikací libovolného typu dle jejich ID.
        /// </summary>
        Dictionary<ulong, IApplication> AllApps { get; }
    }
}
