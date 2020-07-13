using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceProvider : IEventServiceProvider
    {
        private ClosedAPI _api;
        private ILogger _log;

        private EventMaskHandler _maskHandler = new EventMaskHandler();

        public MainMessage HandleMessage(MainMessage msg)
        {
            EventDataMsg dataMsg = msg.EventMsg.EventDataMsg;
            if(dataMsg == null)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid msg type.");
            }

            EventResponse response;
            if (msg.EventMsg.EventDataMsg.AppTypeCase != EventDataMsg.AppTypeOneofCase.None)
            {
                try
                {
                    response = new EventResponse();
                    byte[] responseData = _api.Services.App.HandleEvent(msg);
                    if (responseData != null)
                    {
                        response.Data = ByteString.CopyFrom(responseData);
                    }
                }
                catch (EventErrorException e)
                {
                    response = IEventService.CreateErrorResponse(msg.MsgId, 0, 0, e.Message);
                }
            }
            else
            {
                const string err_msg = "Main server does not handle events of this type.";
                _log.Error(err_msg);
                response = IEventService.CreateErrorResponse(msg.MsgId, 0, 0, err_msg);
            }
            response = _maskHandler.Handle(msg.ClientId, dataMsg, response);
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

    }
}
