using System;
using System.Collections.Generic;
using System.Numerics;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Common.Core.Services.AppService;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Enum příjemce zprávy
    /// </summary>
    public enum AppMsgRecipient
    {
        FORWARDER,
        PROVIDER
    }

    /// <summary>
    /// Interface aplikační služby
    /// </summary>
    public interface IAppServiceClient : IServiceClient
    {
        /// <summary>
        /// Seznam instancí všech zinicializovaných aplikací.
        /// </summary>
        Dictionary<ulong, IApplication> AllApps { get; }

        /// <summary>
        /// Seznam všech zinicializovaných menu aplikací. 
        /// </summary>
        List<IMenuApp> MenuApps { get; }

        /// <summary>
        /// Seznam všech zinicializovaných aplikací na pozadí. 
        /// </summary>
        List<IBackgroundApp> BackgroundApps { get; }

        /// <summary>
        /// Seznam všech zinicializovaných globálních aplikací. 
        /// </summary>
        List<IGlobalAPp> GlobalApps { get; }

        /// <summary>
        /// Seznam všech zinicializovaných objektových aplikací. 
        /// </summary>
        List<IObjectApp> ObjectApps { get; }

        /// <summary>
        /// Slovník všech instancí objektových aplikací s klíčem ID dané aplikace
        /// a s hodnotou seznamu instancí dané objektové aplikace s ID instance jako index v daném seznamu.
        /// </summary>
        Dictionary<ulong, List<IObjectAppInstance>> ObjectAppInstances { get; }

        /// <summary>
        /// Event volaný v případě přidání nové aplikace s info objektem v argumentu.
        /// </summary>
        event Action<AppInfo> AddedNewApp;

        /// <summary>
        /// Event volaný v případě přidání nové instance objektové aplikace s instancí dané instance v argumentu.
        /// </summary>
        event Action<IObjectAppInstance> AddedNewObjectAppInstance;

        /// <summary>
        /// Registrace nezinicializované aplikace.
        /// </summary>
        /// <param name="app">Instance nezinicializované aplikace.</param>
        void RegisterApp(IApplication app);

        /// <summary>
        /// Vytvoření nové instance objektové aplikace.
        /// </summary>
        /// <param name="appId">ID objektové aplikace.</param>
        /// <param name="center">Střed vymezený v prostoru místnosti pro danou instanci.</param>
        void CreateObjectAppInstance(ulong appId, Vector3 center);

        /// <summary>
        /// Vytvoření nové instance objektové aplikace.
        /// </summary>
        /// <param name="app">Instance objektové aplikace.</param>
        /// <param name="center">Střed vymezený v prostoru místnosti pro danou instanci.</param>
        void CreateObjectAppInstance(IObjectApp app, Vector3 center);

        /// <summary>
        /// Vytvoření nové instance objektové aplikace.
        /// </summary>
        /// <param name="appId">ID objektové aplikace.</param>
        /// <param name="appInstanceId">požadované ID vytvořené instance.</param>
        /// <param name="center">Střed vymezený v prostoru místnosti pro danou instanci.</param>
        void CreateObjectAppInstance(ulong appId, ulong appInstanceId, Vector3 center);

        /// <summary>
        /// Vytvoření nové instance objektové aplikace.
        /// </summary>
        /// <param name="app">Instance objektové aplikace.</param>
        /// <param name="appInstanceId">požadované ID vytvořené instance.</param>
        /// <param name="center">Střed vymezený v prostoru místnosti pro danou instanci.</param>
        void CreateObjectAppInstance(IObjectApp app, ulong appInstanceId, Vector3 center);

        /// <summary>
        /// Odeslání zprávy aplikace.
        /// </summary>
        /// <param name="app">AppInfo objekt odesílající aplikace.</param>
        /// <param name="data">data ve formě byte array.</param>
        /// <param name="recipient">Příjemce dat (Provider/Forwarder).</param>
        /// <returns>ServiceCallback s návratovou hodnotou odpovědi ve formě byte array.</returns>
        IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient);

        /// <summary>
        /// Dotaz na seznam všech dostupných balíčků aplikací k instalaci.
        /// </summary>
        /// <returns>Seznam dostupných balíčků aplikací.</returns>
        IServiceCallback<List<IAppPackageInfo>> ListAppPackages();

        /// <summary>
        /// Žádost o instalaci aplikace z balíčkovacího systému.
        /// 
        /// Po dokončení volání je zaručené, že aplikace je již zinicializovaná.
        /// </summary>
        /// <param name="appId">ID aplikace.</param>
        /// <returns>ServiceCallback bez návratové hodnoty.</returns>
        IServiceCallback<bool> LoadApp(ulong appId);
    }
}
