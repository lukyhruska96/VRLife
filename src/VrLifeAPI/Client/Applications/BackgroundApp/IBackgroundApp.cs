using VrLifeAPI.Client.API;

namespace VrLifeAPI.Client.Applications.BackgroundApp
{
    public interface IBackgroundApp : IApplication
    {
        void Init(IOpenAPI api);
    }
}
