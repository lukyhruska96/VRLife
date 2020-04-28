using Google.Protobuf;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services;
using VrLifeServer.Database;
using VrLifeServer.Networking;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer
{
    class MainServer
    {
        private UDPNetworking<MainMessage> udpListenner;
        private List<IService> coreServices = 
            new List<IService>(Enum.GetValues(typeof(MainMessage.MessageTypeOneofCase)).Length);
        private bool prepared = false;
        private OpenAPI _openAPI;
        private ClosedAPI _closedAPI;

        public void Init()
        {
            if (VrLifeServer.Conf.Database.Equals(default(DatabaseConnectionStruct)))
            {
                throw new ArgumentNullException("Database object can't be empty");
            }
            PrepareDatabase();
            PrepareListenner();
            PrepareMainServices();
            prepared = true;
        }

        private void PrepareDatabase()
        {
            using(var db = new VrLifeDbContext())
            {
                if(!(db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                {
                    throw new DatabaseException("Database does not exists or does not have required tables. Please make migration before starting the server.");
                }
            }
        }

        private void PrepareListenner()
        {
            udpListenner = new UDPNetworking<MainMessage>(VrLifeServer.Conf.Address, (int)VrLifeServer.Conf.UdpPort, this.MsgRouter);
        }

        private void PrepareMainServices()
        {
            ServiceProvider sp = new ServiceProvider(
                new SystemService(), 
                new EventService(), 
                new TickRateService(), 
                new RoomService(), 
                new UserService(), 
                new AppService());

            coreServices[(int)MainMessage.MessageTypeOneofCase.SystemMsg] = sp.System;
            coreServices[(int)MainMessage.MessageTypeOneofCase.TickMsg] = sp.TickRate;
            coreServices[(int)MainMessage.MessageTypeOneofCase.EventMsg] = sp.Event;
            coreServices[(int)MainMessage.MessageTypeOneofCase.RoomMsg] = sp.Room;
            coreServices[(int)MainMessage.MessageTypeOneofCase.UserMsg] = sp.User;
            coreServices[(int)MainMessage.MessageTypeOneofCase.AppMsg] = sp.App;

            // APIs
            _openAPI = new OpenAPI(udpListenner, VrLifeServer.Conf.Loggers);
            _closedAPI = new ClosedAPI(_openAPI, sp);

            // Initialization of services
            foreach (IService service in coreServices)
            {
                service.Init(_closedAPI);
            }
        }

        public void Start()
        {
            if(!prepared)
            {
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
