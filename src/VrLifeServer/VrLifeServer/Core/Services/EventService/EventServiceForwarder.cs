using System;
using System.Collections.Generic;
using System.Diagnostics;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;
using VrLifeShared.Core.Services.EventService;
using VrLifeServer.Core.Services.TickRateService;
using System.Linq;

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
    class EventServiceForwarder : IEventServiceForwarder
    {
        private Dictionary<ulong, EventMask> _eventMasks = new Dictionary<ulong, EventMask>();

        private ClosedAPI _api;
        private ILogger _log;
        public MainMessage HandleMessage(MainMessage msg)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            EventMsg eventMsg = msg.EventMsg;
            if(eventMsg.EventDataMsg == null)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Forwarder can process only EventDataMsg type.");
            }
            MainMessage response;
            if (eventMsg.EventDataMsg.AppTypeCase != EventDataMsg.AppTypeOneofCase.None)
            {
                ulong? userId = _api.Services.User.GetUserIdByClientId(msg.ClientId);
                if(!userId.HasValue)
                {
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unauthenticated client.");
                }
                response = _api.Services.App.HandleEvent(eventMsg, userId.Value, eventMsg.EventDataMsg.InstanceId);
            }
            else
            {
                response = HandleEvent(msg);
            }
            // process time calculation for RTT
            if(response.EventMsg != null && response.EventMsg.EventResponse != null)
            {
                response.EventMsg.EventResponse.ProcessTime = (uint)sw.ElapsedMilliseconds;
            }
            return response;
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

        public MainMessage HandleEvent(MainMessage msg)
        {
            EventDataMsg eventMsg = msg.EventMsg.EventDataMsg;
            if (eventMsg == null || !Enum.IsDefined(typeof(EventType), (int)eventMsg.EventType))
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown event type.");
            }
            EventType eventType = (EventType)eventMsg.EventType;
            switch (eventType)
            {
                case EventType.SKELETON_STATE:
                    return HandleSkeletonEvent(msg.MsgId, msg.ClientId, eventMsg);
                case EventType.OBJECT_STATE:
                    return HandleGameObjectEvent(msg.MsgId, msg.ClientId, eventMsg);
                default:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Wrong event type.");
            }
        }

        private MainMessage HandleSkeletonEvent(ulong msgId, ulong clientId, EventDataMsg eventMsg)
        {
            Skeleton skeleton = eventMsg.SkeletonValue;
            if(skeleton == null)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "Skeleton value cannot be null.");
            }
            if(!_api.Services.User.FastCheckUserId(clientId, skeleton.UserId))
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "Authentication error.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(skeleton.UserId);
            if (!roomId.HasValue)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "User is not connected to any room.");
            }
            SkeletonState skelState = new SkeletonState(skeleton);
            _log.Debug($"Received SkeletonEvent for room {roomId.Value} from userID {skeleton.UserId}");
            _api.Services.TickRate.SetSkeletonState(roomId.Value, skeleton.UserId, skelState);
            return CreateEventResponse(msgId, clientId, eventMsg);
        }

        private MainMessage HandleGameObjectEvent(ulong msgId, ulong clientId, EventDataMsg eventMsg)
        {
            GameObject gameObject = eventMsg.ObjectValue;
            if(gameObject == null)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "GameObject value cannot be null.");
            }

            return CreateEventResponse(msgId, clientId, eventMsg);
        }

        private MainMessage CreateEventResponse(ulong msgId, ulong clientId, EventDataMsg eventMsg)
        {
            if(!_eventMasks.TryGetValue(clientId, out EventMask eventMask))
            {
                lock(_eventMasks)
                {
                    eventMask = new EventMask();
                    _eventMasks[clientId] = eventMask;
                }
            }
            lock(eventMask)
            {
                if(eventMsg.EventId > eventMask.HighestId)
                {
                    eventMask.Mask = eventMask.Mask << (int)(eventMsg.EventId - eventMask.HighestId);
                    eventMask.Mask |= 0x01;
                    eventMask.HighestId = eventMsg.EventId;
                }
                else if(eventMask.HighestId - eventMsg.EventId < 32)
                {
                    eventMask.Mask |= (uint)0x01 << (int)(eventMask.HighestId - eventMsg.EventId);
                }
                EventResponse eventResponse = new EventResponse();
                eventResponse.EventMask = eventMask.Mask;
                eventResponse.HighestEventId = eventMask.HighestId;
                MainMessage msg = new MainMessage();
                msg.EventMsg = new EventMsg();
                msg.EventMsg.EventResponse = eventResponse;
                return msg;
            }
        }
    }
}
