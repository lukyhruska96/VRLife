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
using Google.Protobuf;
using System.Reflection.Metadata;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceForwarder : IEventServiceForwarder
    {
        private EventMaskHandler _maskHandler = new EventMaskHandler();

        private ClosedAPI _api;
        private ILogger _log;

        public MainMessage HandleMessage(MainMessage msg)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            EventDataMsg eventMsg = msg.EventMsg.EventDataMsg;
            EventResponse response = new EventResponse();
            if (eventMsg == null)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Forwarder can process only EventDataMsg type.");
            }
            if (eventMsg.AppTypeCase != EventDataMsg.AppTypeOneofCase.None)
            {
                try
                {
                    byte[] responseData = _api.Services.App.HandleEvent(msg);
                    if (responseData != null)
                    {
                        response.Data = ByteString.CopyFrom(responseData);
                    }
                }
                catch(EventErrorException e)
                {
                    response = IEventService.CreateErrorResponse(msg.MsgId, 0, 0, e.Message);
                }
            }
            else
            {
                EventResponse handleResponse = HandleEvent(msg);
                if (handleResponse != null)
                {
                    response = handleResponse;
                }
            }

            response.ProcessTime = (uint)sw.ElapsedMilliseconds;
            response = _maskHandler.Handle(msg.ClientId, eventMsg, response);

            MainMessage responseMsg = new MainMessage();
            responseMsg.EventMsg = new EventMsg();
            responseMsg.EventMsg.EventResponse = response;
            return responseMsg;
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

        public EventResponse HandleEvent(MainMessage msg)
        {
            EventDataMsg eventMsg = msg.EventMsg.EventDataMsg;
            EventType eventType = (EventType)eventMsg.EventType;
            switch (eventType)
            {
                case EventType.SKELETON_STATE:
                    return HandleSkeletonEvent(msg.MsgId, msg.ClientId, eventMsg);
                case EventType.OBJECT_STATE:
                    return HandleGameObjectEvent(msg.MsgId, msg.ClientId, eventMsg);
                default:
                    return IEventService.CreateErrorResponse(msg.MsgId, 0, 0, "Wrong event type.");
            }
        }

        private EventResponse HandleSkeletonEvent(ulong msgId, ulong clientId, EventDataMsg eventMsg)
        {
            Skeleton skeleton = eventMsg.SkeletonValue;
            if(skeleton == null)
            {
                return IEventService.CreateErrorResponse(msgId, 0, 0, "Skeleton value cannot be null.");
            }
            if(!_api.Services.User.FastCheckUserId(clientId, skeleton.UserId))
            {
                return IEventService.CreateErrorResponse(msgId, 0, 0, "Authentication error.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(skeleton.UserId);
            if (!roomId.HasValue)
            {
                return IEventService.CreateErrorResponse(msgId, 0, 0, "User is not connected to any room.");
            }
            SkeletonState skelState = new SkeletonState(skeleton);
            _api.Services.TickRate.SetSkeletonState(roomId.Value, skeleton.UserId, skelState);
            return null;
        }

        private EventResponse HandleGameObjectEvent(ulong msgId, ulong clientId, EventDataMsg eventMsg)
        {
            GameObject gameObject = eventMsg.ObjectValue;
            if(gameObject == null)
            {
                return IEventService.CreateErrorResponse(msgId, 0, 0, "GameObject value cannot be null.");
            }
            return null;
        }
    }
}
