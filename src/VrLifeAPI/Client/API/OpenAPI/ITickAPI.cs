using VrLifeAPI.Client.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface ITickAPI
    {

        IServiceCallback<SnapshotData> GetSnapshot();

        SnapshotData[] GetSnapshotBuffer();
    }
}
