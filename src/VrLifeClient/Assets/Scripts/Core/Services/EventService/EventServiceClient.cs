using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.EventService;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Core.Services.EventService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.EventService
{
    class EventServiceClient : IServiceClient
    {
        private ClosedAPI _api;
        private ConcurrentQueue<EventDataMsg> _eventBuffer = new ConcurrentQueue<EventDataMsg>();
        private ulong _eventsSent = 0;
        private object _eventLock = new object();

        private uint _lastRTT = 0;
        public uint LastRTT { get => _lastRTT; }
        

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
        }

        public ServiceCallback<bool> SendSkeleton(SkeletonState skeleton)
        {
            skeleton.UserId = _api.Services.User.UserId.Value;
            EventDataMsg eventData = new EventDataMsg();
            eventData.EventType = (uint)VrLifeShared.Core.Services.EventService.EventType.SKELETON_STATE;
            eventData.SkeletonValue = skeleton.ToNetworkModel();
            return SendEvent(eventData);
        }

        public ServiceCallback<bool> SendEvent(EventDataMsg eventData)
        {
            return new ServiceCallback<bool>(() =>
            {
                if(_api.Services.Room.ForwarderAddress == null)
                {
                    return false;
                }
                lock(_eventLock)
                {
                    eventData.EventId = _eventsSent++;
                    _eventBuffer.Enqueue(eventData);
                }
                MainMessage msg = new MainMessage();
                msg.EventMsg = new EventMsg();
                msg.EventMsg.EventDataMsg = eventData;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.Services.Room.ForwarderAddress);
                sw.Stop();
                if(SystemServiceClient.IsErrorMsg(response))
                {
                    throw new EventServiceException(response.SystemMsg.ErrorMsg.ErrorMsg_);
                }
                EventResponse eventResponse = response.EventMsg.EventResponse;
                if(eventResponse == null)
                {
                    throw new EventServiceException("Unknown response.");
                }
                _lastRTT = (uint)(sw.ElapsedMilliseconds - (long)eventResponse.ProcessTime);
                HandleEventResponse(eventResponse);
                return true;
            });
            
        }

        private void HandleEventResponse(EventResponse response)
        {
            bool[] maskValues = response.EventMask.ToBinary();
            EventDataMsg[] eventData;
            ulong highestId;
            lock(_eventLock)
            {
                highestId = _eventsSent;
                eventData = _eventBuffer.ToArray();
            }
            for(int i = 0; i < maskValues.Length; ++i)
            {
                if(!maskValues[i])
                {
                    ulong eventId = (ulong)((long)response.HighestEventId - (maskValues.Length - i - 1));
                    if((long)(highestId - eventId) < eventData.Length)
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
    }
}
