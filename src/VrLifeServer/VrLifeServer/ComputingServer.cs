using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.TickRateService;
using VrLifeServer.Core.Services.UserService;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Networking;
using VrLifeServer.Networking.NetworkingModels;
using System.Linq;
using VrLifeServer.Logging;

namespace VrLifeServer
{
    class ComputingServer
    {

        private UDPNetworking<MainMessage> udpListenner;
        private List<IService> coreServices =
            new List<IService>();
        private OpenAPI _openAPI;
        private ClosedAPI _closedAPI;
        private Config _config;
        private ILogger _log;
        private uint serverId = 0;

        public void Init(Config config)
        {
            this._config = config;
            this._log = new LoggerWrapper(this.GetType().Name, this._config.Loggers);
            this._log.Debug("Logger initialized.");
            udpListenner = new UDPNetworking<MainMessage>(_config.Address, (int)_config.UdpPort, this.MsgRouter);
        }

        public void Start()
        {
            this._log.Debug("In method Start().");
            // say hi
            this._log.Info("Contacting Main Server...");
            udpListenner.Send(ISystemService.CreateHelloMessage(),
                _config.MainServer,
                this.AfterFirstResponse,
                (e) =>
                {
                    _log.Error(new ServerException("Main Server is unreachable", e));
                    Environment.Exit(1);
                });
        }

        private void AfterFirstResponse(MainMessage msg)
        {
            this._log.Info("Received response from Main Server.");
            SystemMsg sysMsg = msg.SystemMsg;
            if(sysMsg.SystemMsgTypeCase.Equals(SystemMsg.SystemMsgTypeOneofCase.ErrorMsg))
            {
                throw new ServerException(sysMsg.ErrorMsg.ErrorMsg_);
            }
            serverId = msg.ServerId;
            this._log.Info("Assigned ID: " + serverId.ToString());
            this._log.Info("Preparing Services...");

            ServiceProvider sp = new ServiceProvider(
                new SystemServiceForwarder(), 
                new EventServiceForwarder(), 
                new TickRateServiceForwarder(), 
                new RoomServiceForwarder(), 
                new UserServiceForwarder(), 
                new AppServiceProvider());

            int maxIndex = Enum.GetValues(typeof(MainMessage.MessageTypeOneofCase))
                .OfType<MainMessage.MessageTypeOneofCase>()
                .Select(x => (int)x)
                .Max();
            coreServices.InsertRange(0, Enumerable.Repeat<IService>(null, maxIndex + 1));
            coreServices[(int)MainMessage.MessageTypeOneofCase.SystemMsg] = sp.System;
            coreServices[(int)MainMessage.MessageTypeOneofCase.TickMsg] = sp.TickRate;
            coreServices[(int)MainMessage.MessageTypeOneofCase.EventMsg] = sp.Event;
            coreServices[(int)MainMessage.MessageTypeOneofCase.RoomMsg] = sp.Room;
            coreServices[(int)MainMessage.MessageTypeOneofCase.UserMsg] = sp.User;
            coreServices[(int)MainMessage.MessageTypeOneofCase.AppMsg] = sp.App;

            // APIs
            _openAPI = new OpenAPI(udpListenner, _config);
            _closedAPI = new ClosedAPI(_openAPI, sp);

            // Initialization of services
            foreach (IService service in coreServices)
            {
                service?.Init(_closedAPI);
            }

            this._log.Info("All services have been initialized.");

            udpListenner.StartListening();
        }

        private MainMessage MsgRouter(MainMessage msg)
        {
            MainMessage response;
            switch(msg.SystemMsg.SystemMsgTypeCase)
            {
                default:
                    response = ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown message type");
                    break;
            }
            response.ServerId = serverId;
            return response;
        }
    }
}
