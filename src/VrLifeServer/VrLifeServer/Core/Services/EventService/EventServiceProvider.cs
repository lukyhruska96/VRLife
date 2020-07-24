using Google.Protobuf;
using System;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.EventService;
using VrLifeServer.API.Provider;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceProvider : IEventServiceProvider
    {
        private IClosedAPI _api;
        private ILogger _log;

        private EventMaskHandler _maskHandler = new EventMaskHandler();

        public MainMessage HandleMessage(MainMessage msg)
        {
            EventDataMsg dataMsg = msg.EventMsg.EventDataMsg;
            if(dataMsg == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid msg type.");
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
                    _log.Error(e);
                    response = VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorResponse(msg.MsgId, 0, 0, e.Message);
                }
            }
            else
            {
                const string err_msg = "Main server does not handle events of this type.";
                _log.Error(err_msg);
                response = VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorResponse(msg.MsgId, 0, 0, err_msg);
            }
            response = _maskHandler.Handle(msg.ClientId, dataMsg, response);
            MainMessage responseMsg = new MainMessage();
            responseMsg.EventMsg = new EventMsg();
            responseMsg.EventMsg.EventResponse = response;
            return responseMsg;
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

    }
}
