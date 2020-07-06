using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.EventService;
using VrLifeShared.Core.Services.EventService;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.API.OpenAPI
{
    class EventAPI
    {
        private EventServiceClient _eventService;
        public EventAPI(EventServiceClient eventService)
        {
            this._eventService = eventService;
        }

        public ServiceCallback<bool> SendSkeleton(SkeletonState skeleton)
        {
            return _eventService.SendSkeleton(skeleton);
        }
    }
}
