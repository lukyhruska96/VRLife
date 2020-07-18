using Assets.Scripts.API;
using System;
using System.Collections.Generic;
using System.Linq;
using VrLifeClient.API;
using VrLifeClient.API.OpenAPI;
using VrLifeClient.Core.Services;
using VrLifeClient.Core.Services.AppService;
using VrLifeClient.Core.Services.EventService;
using VrLifeClient.Core.Services.RoomService;
using VrLifeClient.Core.Services.SystemService;
using VrLifeClient.Core.Services.TickRateService;
using VrLifeClient.Core.Services.UserService;
using VrLifeShared.Networking;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient
{
    class VrLifeClient : IDisposable
    {
        private UDPNetworking<MainMessage> _client;
        public UDPNetworking<MainMessage> Client { get => _client; }

        private OpenAPI _openAPI;
        public OpenAPI OpenAPI { get => _openAPI; }

        private ClosedAPI _closedAPI;

        public ClosedAPI ClosedAPI { get => _closedAPI; }

        private List<IServiceClient> coreServices = new List<IServiceClient>();

        private Config _config;

        private MiddlewareProvider _middlewareProvider;
        public MiddlewareProvider Middlewares { get => _middlewareProvider; }

        public VrLifeClient(Config config)
        {
            this._config = config;
            _middlewareProvider = new MiddlewareProvider(
                new ClientIdFiller(),
                new MsgIdIncrement(),
                new RedirectMsgHandler());
            _client = new UDPNetworking<MainMessage>(_middlewareProvider.ToList());
        }

        public void Init()
        {
            ServiceProvider sp = new ServiceProvider(
                new AppServiceClient(),
                new EventServiceClient(),
                new RoomServiceClient(),
                new SystemServiceClient(),
                new TickRateServiceClient(),
                new UserServiceClient()
                );
            int maxIndex = Enum.GetValues(typeof(MainMessage.MessageTypeOneofCase))
                .OfType<MainMessage.MessageTypeOneofCase>()
                .Select(x => (int)x)
                .Max();
            coreServices.InsertRange(0, Enumerable.Repeat<IServiceClient>(null, maxIndex + 1));
            coreServices[(int)MainMessage.MessageTypeOneofCase.SystemMsg] = sp.System;
            coreServices[(int)MainMessage.MessageTypeOneofCase.TickMsg] = sp.TickRate;
            coreServices[(int)MainMessage.MessageTypeOneofCase.EventMsg] = sp.Event;
            coreServices[(int)MainMessage.MessageTypeOneofCase.RoomMsg] = sp.Room;
            coreServices[(int)MainMessage.MessageTypeOneofCase.UserMngMsg] = sp.User;
            coreServices[(int)MainMessage.MessageTypeOneofCase.AppMsg] = sp.App;

            // APIs
            _openAPI = new OpenAPI(_client, _config, sp);
            _closedAPI = new ClosedAPI(_openAPI, sp, _middlewareProvider);

            // Initialization of services
            foreach (IServiceClient service in coreServices)
            {
                service?.Init(_closedAPI);
            }
        }

        public void Dispose()
        {
            _closedAPI?.Dispose();
        }
    }
}
