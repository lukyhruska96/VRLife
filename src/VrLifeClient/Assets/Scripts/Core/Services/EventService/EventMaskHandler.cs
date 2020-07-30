using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API;

namespace Assets.Scripts.Core.Services.EventService
{
    class EventMaskHandler
    {
        private IClosedAPI _api;
        private ConcurrentQueue<EventDataMsg> _eventBuffer = new ConcurrentQueue<EventDataMsg>();
        private ulong _eventsSent = 0;
        private object _eventLock = new object();
        public EventMaskHandler(IClosedAPI api)
        {
            _api = api;
        }

        public void SetId(EventDataMsg eventMsg)
        {
            lock(_eventLock)
            {
                eventMsg.EventId = _eventsSent++;
                _eventBuffer.Enqueue(eventMsg);
            }
        }

        public void HandleEventResponse(EventResponse response)
        {
            bool[] maskValues = response.EventMask.ToBinary();
            EventDataMsg[] eventData;
            ulong highestId;
            lock (_eventLock)
            {
                highestId = _eventsSent;
                eventData = _eventBuffer.ToArray();
            }
            for (int i = 0; i < maskValues.Length; ++i)
            {
                if (!maskValues[i])
                {
                    ulong eventId = (ulong)((long)response.HighestEventId - (maskValues.Length - i - 1));
                    if((int)eventId >= eventData.Length)
                    {
                        continue;
                    }
                    if ((long)(highestId - eventId) < eventData.Length)
                    {
                        ulong evIdx = (ulong)(eventData.Length - (int)(highestId - eventId) - 1);
                        MainMessage msg = new MainMessage();
                        msg.EventMsg = new EventMsg();
                        msg.EventMsg.EventDataMsg = eventData[evIdx];
                        _api.OpenAPI.Networking.SendAsync(msg, _api.Services.Room.ForwarderAddress, _ => { });
                    }
                }
            }
        }

        public void Reset()
        {
            lock(_eventLock)
            {
                _eventsSent = 0;
                _eventBuffer = new ConcurrentQueue<EventDataMsg>();
            }
        }
    }
}
