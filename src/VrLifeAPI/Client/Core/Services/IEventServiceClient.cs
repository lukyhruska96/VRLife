using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    public enum EventRecipient
    {
        FORWARDER,
        PROVIDER
    }

    public interface IEventServiceClient : IServiceClient
    {
        uint LastRTT { get; }

        IServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton);

        IServiceCallback<byte[]> SendEvent(EventDataMsg eventData, EventRecipient recipient);

        EventResponse SendEventData(EventDataMsg eventData, EventRecipient recipient);
    }
}
