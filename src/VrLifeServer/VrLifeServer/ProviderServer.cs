using Google.Protobuf;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.TickRateService;
using VrLifeServer.Core.Services.UserService;
using VrLifeServer.Database;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.Middlewares;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Provider.Database;
using VrLifeAPI.Networking.Middlewares;
using MySql.Data.MySqlClient;
using System;

namespace VrLifeServer
{
    class ProviderServer
    {
        private UDPNetworking<MainMessage> udpListenner;
        private List<VrLifeAPI.Provider.Core.Services.IServiceProvider> coreServices = 
            new List<VrLifeAPI.Provider.Core.Services.IServiceProvider>();
        private bool prepared = false;
        private OpenAPI _openAPI;
        private ClosedAPI _closedAPI;
        private Config _config;
        private ILogger _log;

        public void Init(Config conf)
        {
            _config = conf;
            _log = new LoggerWrapper(this.GetType().Name, _config.Loggers);
            _log.Debug("Main Server logger is set.");
            if (_config.Database.Equals(default(DatabaseConnectionStruct)))
            {
                _log.Debug("Database object is empty.");
                throw new System.ArgumentNullException("Database object can't be empty");
            }
            _log.Debug("Preparing databse...");
            PrepareDatabase();
            _log.Debug("Preparing listenner...");
            PrepareListenner();
            _log.Debug("Preparing Main Services...");
            PrepareMainServices();
            _log.Debug("Main Server is ready.");
            prepared = true;
        }

        private void PrepareDatabase()
        {
            using(var db = new VrLifeDbContext())
            {
                try
                {
                    if (!db.Database.CanConnect())
                    {
                        _log.Error("Cannot connect to database.");
                        throw new DatabaseException("Cannot connect to database.");
                    }
                }
                catch(InvalidOperationException)
                {
                    _log.Error("Cannot connect to database.");
                    throw new DatabaseException("Cannot connect to database.");
                }
                if(!(db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                {
                    throw new DatabaseException("Database does not exists or does not have required tables. Please make migration before starting the server.");
                }
            }
        }

        private void PrepareListenner()
        {
            var middlewares = new List<IMiddleware<MainMessage>>();
            middlewares.Add(new MsgIdIncrement());
            middlewares.Add(new ServerIdFiller());
            udpListenner = new UDPNetworking<MainMessage>(_config.Listen, (int)_config.UdpPort, this.MsgRouter, middlewares);
            udpListenner.SetDebug(_config.Debug);
        }

        private void PrepareMainServices()
        {
            ServiceProvider sp = new ServiceProvider(
                new SystemServiceProvider(), 
                new EventServiceProvider(), 
                new TickRateServiceProvider(), 
                new RoomServiceProvider(), 
                new UserServiceProvider(), 
                new AppServiceProvider());

            int maxIndex = System.Enum.GetValues(typeof(MainMessage.MessageTypeOneofCase))
                .OfType<MainMessage.MessageTypeOneofCase>()
                .Select(x => (int)x)
                .Max();
            coreServices.InsertRange(0, Enumerable.Repeat<VrLifeAPI.Provider.Core.Services.IServiceProvider>(null, maxIndex+1));
            coreServices[(int)MainMessage.MessageTypeOneofCase.SystemMsg] = sp.System;
            coreServices[(int)MainMessage.MessageTypeOneofCase.TickMsg] = sp.TickRate;
            coreServices[(int)MainMessage.MessageTypeOneofCase.EventMsg] = sp.Event;
            coreServices[(int)MainMessage.MessageTypeOneofCase.RoomMsg] = sp.Room;
            coreServices[(int)MainMessage.MessageTypeOneofCase.UserMngMsg] = sp.User;
            coreServices[(int)MainMessage.MessageTypeOneofCase.AppMsg] = sp.App;

            // APIs
            _openAPI = new OpenAPI(udpListenner, _config);
            _closedAPI = new ClosedAPI(_openAPI, sp);
            _openAPI.Init(_closedAPI);

            // Initialization of services
            foreach (VrLifeAPI.Provider.Core.Services.IServiceProvider service in coreServices)
            {
                service?.Init(_closedAPI);
            }
        }

        public void Start()
        {
            _log.Debug("in method Start()");
            if (!prepared)
            {
                _log.Info("Main Server is not ready.");
                throw new ServerException("Server is not prepared. Call Init function first.");
            }
            udpListenner.StartListening();
        }

        private MainMessage MsgRouter(MainMessage msg)
        {
            return coreServices[(int)msg.MessageTypeCase].HandleMessage(msg);
        }
    }
}
