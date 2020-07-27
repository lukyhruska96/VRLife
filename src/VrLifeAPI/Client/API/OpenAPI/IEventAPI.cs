using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface IEventAPI
    {
        IServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton);

        IServiceCallback<byte[]> SendEvent(EventDataMsg msg, EventRecipient recipient);
    }
}
