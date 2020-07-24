
using VrLifeAPI.Provider.API.APIs;

namespace VrLifeAPI.Provider.API
{
    public interface IClosedAPI
    {
        IOpenAPI OpenAPI { get; }
        IServiceProvider Services { get; }
    }
}
