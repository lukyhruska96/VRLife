using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.TickRateService;
using VrLifeServer.Core.Services.UserService;
using VrLifeServer.Core.Services.AppService;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;
using System.Linq;
using VrLifeShared.Logging;
using VrLifeShared.Networking.Middlewares;

namespace VrLifeServer
{
    class ForwarderServer
    {

        private UDPNetworking<MainMessage> udpListenner;
        private List<IServiceForwarder> coreServices =
            new List<IServiceForwarder>();
        private OpenAPI _openAPI;
        private ClosedAPI _closedAPI;
        private Config _config;
        private ILogger _log;
        private uint _serverId = 0;
        private ServerIdFiller _serverIdFiller = new ServerIdFiller();
        private ServiceProvider _serviceProvider;

        public void Init(Config config)
        {
            this._config = config;
            this._log = new LoggerWrapper(this.GetType().Name, this._config.Loggers);
            this._log.Debug("Logger initialized.");
            var middlewares = new List<IMiddleware<MainMessage>>();
            middlewares.Add(new MsgIdIncrement());
            middlewares.Add(_serverIdFiller);
            udpListenner = new UDPNetworking<MainMessage>(_config.Listen, (int)_config.UdpPort, this.MsgRouter, middlewares);
        }

        public void Start()
        {
            this._log.Debug("In method Start().");
            // say hi
            this._log.Info("Contacting Main Server...");
            udpListenner.SendAsync(ISystemService.CreateHelloMessage(_config),
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
            _serverId = msg.ServerId;
            _serverIdFiller.SetId(_serverId);
            this._log.Info("Assigned ID: " + _serverId.ToString());
            this._log.Info("Preparing Services...");

            _serviceProvider = new ServiceProvider(
                new SystemServiceForwarder(), 
                new EventServiceForwarder(), 
                new TickRateServiceForwarder(), 
                new RoomServiceForwarder(), 
                new UserServiceForwarder(), 
                new AppServiceForwarder());

            int maxIndex = Enum.GetValues(typeof(MainMessage.MessageTypeOneofCase))
                .OfType<MainMessage.MessageTypeOneofCase>()
                .Select(x => (int)x)
                .Max();
            coreServices.InsertRange(0, Enumerable.Repeat<IServiceForwarder>(null, maxIndex + 1));
            coreServices[(int)MainMessage.MessageTypeOneofCase.SystemMsg] = _serviceProvider.System;
            coreServices[(int)MainMessage.MessageTypeOneofCase.TickMsg] = _serviceProvider.TickRate;
            coreServices[(int)MainMessage.MessageTypeOneofCase.EventMsg] = _serviceProvider.Event;
            coreServices[(int)MainMessage.MessageTypeOneofCase.RoomMsg] = _serviceProvider.Room;
            coreServices[(int)MainMessage.MessageTypeOneofCase.UserMngMsg] = _serviceProvider.User;
            coreServices[(int)MainMessage.MessageTypeOneofCase.AppMsg] = _serviceProvider.App;

            // APIs
            _openAPI = new OpenAPI(udpListenner, _config);
            _closedAPI = new ClosedAPI(_openAPI, _serviceProvider);

            // Initialization of services
            foreach (IServiceForwarder service in coreServices)
            {
                service?.Init(_closedAPI);
            }

            this._log.Info("All services have been initialized.");

            udpListenner.StartListening();
        }

        private MainMessage MsgRouter(MainMessage msg)
        {
            return coreServices[(int)msg.MessageTypeCase].HandleMessage(msg);
        }
    }
}
