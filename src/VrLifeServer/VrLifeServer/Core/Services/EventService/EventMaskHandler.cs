using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    class EventMask
    {
        public ulong HighestId { get; set; }
        public uint Mask { get; set; }

        public EventMask()
        {
            HighestId = 0;
            Mask = uint.MaxValue;
        }
    }

    class EventMaskHandler
    {
        private Dictionary<ulong, EventMask> _eventMasks = new Dictionary<ulong, EventMask>();

        public EventResponse Handle(ulong clientId, EventDataMsg eventMsg, EventResponse response)
        {

            if (!_eventMasks.TryGetValue(clientId, out EventMask eventMask))
            {
                lock (_eventMasks)
                {
                    eventMask = new EventMask();
                    _eventMasks[clientId] = eventMask;
                }
            }
            lock (eventMask)
            {
                if(eventMsg.EventId == 0)
                {
                    lock(_eventMasks)
                    {
                        _eventMasks[clientId] = new EventMask();
                    }
                }
                if (eventMsg.EventId > eventMask.HighestId)
                {
                    eventMask.Mask = eventMask.Mask << (int)(eventMsg.EventId - eventMask.HighestId);
                    eventMask.Mask |= 0x01;
                    eventMask.HighestId = eventMsg.EventId;
                }
                else if (eventMask.HighestId - eventMsg.EventId < 32)
                {
                    eventMask.Mask |= (uint)0x01 << (int)(eventMask.HighestId - eventMsg.EventId);
                }
                response.EventMask = eventMask.Mask;
                response.HighestEventId = eventMask.HighestId;
                return response;
            }
        }
    }
}
