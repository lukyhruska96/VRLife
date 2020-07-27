using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.Core.Services.EventService;

namespace VrLifeClient.API.OpenAPI
{
    class EventAPI : IEventAPI
    {
        private IEventServiceClient _eventService;
        public EventAPI(IEventServiceClient eventService)
        {
            this._eventService = eventService;
        }

        public IServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton)
        {
            return _eventService.SendSkeleton(skeleton);
        }

        public IServiceCallback<byte[]> SendEvent(EventDataMsg msg, EventRecipient recipient) {
            return _eventService.SendEvent(msg, recipient);
        }
    }
}
