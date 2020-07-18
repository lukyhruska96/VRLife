using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.EventService;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Core.Services.EventService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.EventService
{
    public enum EventRecipient
    {
        FORWARDER,
        PROVIDER
    }
    class EventServiceClient : IServiceClient
    {
        private ClosedAPI _api;

        private uint _lastRTT = 0;
        private EventMaskHandler _providerHandler;
        private EventMaskHandler _forwarderHandler;
        public uint LastRTT { get => _lastRTT; }
        

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
            _providerHandler = new EventMaskHandler(_api);
            _forwarderHandler = new EventMaskHandler(_api);
            _api.Services.Room.RoomExited += Reset;
        }

        public ServiceCallback<byte[]> SendSkeleton(SkeletonState skeleton)
        {
            if(!_api.Services.User.UserId.HasValue)
            {
                throw new EventServiceException("UserId cannot be null.");
            }
            skeleton.UserId = _api.Services.User.UserId.Value;
            EventDataMsg eventData = new EventDataMsg();
            eventData.EventType = (uint)VrLifeShared.Core.Services.EventService.EventType.SKELETON_STATE;
            eventData.SkeletonValue = skeleton.ToNetworkModel();
            return SendEvent(eventData, EventRecipient.FORWARDER);
        }

        public ServiceCallback<byte[]> SendEvent(EventDataMsg eventData, EventRecipient recipient)
        {
            return new ServiceCallback<byte[]>(() =>
            {
                switch(recipient)
                {
                    case EventRecipient.FORWARDER:
                        _forwarderHandler.SetId(eventData);
                        break;
                    case EventRecipient.PROVIDER:
                        _providerHandler.SetId(eventData);
                        break;
                        
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                EventResponse response = SendEventData(eventData, recipient);
                sw.Stop();
                if (response == null)
                {
                    throw new EventServiceException("Unknown response.");
                }
                if (response.HasDataCase == EventResponse.HasDataOneofCase.Error)
                {
                    throw new EventServiceException(response.Error.ErrorMsg_);
                }
                _lastRTT = (uint)(sw.ElapsedMilliseconds - (long)response.ProcessTime);
                switch(recipient)
                {
                    case EventRecipient.FORWARDER:
                        _forwarderHandler.HandleEventResponse(response);
                        break;
                    case EventRecipient.PROVIDER:
                        _providerHandler.HandleEventResponse(response);
                        break;
                }
                return response.Data.ToByteArray();
            });
        }

        public EventResponse SendEventData(EventDataMsg eventData, EventRecipient recipient)
        {
            IPEndPoint address = recipient == EventRecipient.FORWARDER ? 
                _api.Services.Room.ForwarderAddress : _api.OpenAPI.Config.MainServer;
            MainMessage msg = new MainMessage();
            msg.EventMsg = new EventMsg();
            msg.EventMsg.EventDataMsg = eventData;
            try
            {
                MainMessage response = _api.OpenAPI.Networking.Send(msg, address);
                return response.EventMsg.EventResponse;
            }
            catch (SocketException)
            {
                _api.Services.System.OnForwarderLost();
                throw;
            }
            
        }

        private void Reset()
        {
            _forwarderHandler.Reset();
        }
    }
}
