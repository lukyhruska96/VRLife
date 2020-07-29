using Assets.Scripts.API;
using VrLifeAPI.Client.API.ClosedAPI;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeClient.API.HUDAPI;

namespace VrLifeAPI.Client.API
{
    /// <summary>
    /// Uzavřené API dostupné pouze pro služby a privilegované aplikace.
    /// Uchovává instance všech služeb, aplikací a ostatních API.
    /// </summary>
    public interface IClosedAPI : System.IDisposable
    {
        /// <summary>
        /// Instance OpenAPI.
        /// </summary>
        IOpenAPI OpenAPI { get; }
        /// <summary>
        /// Instance poskytovatele instancí služeb.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Instance poskytovatele všech middlewarů objektu IUDPNetworking.
        /// </summary>
        IMiddlewareProvider Middlewares { get; }

        /// <summary>
        /// Instance MenuAPI.
        /// </summary>
        IMenuAPI MenuAPI { get; }

        /// <summary>
        /// Instance HUDAPI.
        /// </summary>
        IHUDAPI HUDAPI { get; }

        /// <summary>
        /// Instance GlobalAPI.
        /// </summary>
        IGlobalAPI GlobalAPI { get; }

        /// <summary>
        /// Instance ObjectAPI.
        /// </summary>
        IObjectAPI ObjectAPI { get; }

        /// <summary>
        /// Instance DeviceAPI.
        /// </summary>
        IDeviceAPI DeviceAPI { get; }
    }
}
