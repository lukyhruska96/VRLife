using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.GlobalAPI;

namespace VrLifeAPI.Client.Applications.GlobalApp
{
    public interface IGlobalApp : IApplication
    {
        void Init(IOpenAPI api, IGlobalAPI globalAPI);
    }
}
