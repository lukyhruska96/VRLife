using Assets.Scripts.API;
using VrLifeAPI.Client.API.ClosedAPI;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeClient.API.HUDAPI;

namespace VrLifeAPI.Client.API
{
    public interface IClosedAPI : System.IDisposable
    {
        IOpenAPI OpenAPI { get; }
        IServiceProvider Services { get; }
        IMiddlewareProvider Middlewares { get; }
        IMenuAPI MenuAPI { get; }
        IHUDAPI HUDAPI { get; }
        IGlobalAPI GlobalAPI { get; }
        IObjectAPI ObjectAPI { get; }
        IDeviceAPI DeviceAPI { get; }
    }
}
