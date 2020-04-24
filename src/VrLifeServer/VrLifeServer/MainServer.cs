using Google.Protobuf;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services;
using VrLifeServer.Database;
using VrLifeServer.Networking;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer
{
    class MainServer
    {
        private UDPNetworking<MainMessage> udpListenner;
        private List<IService> coreServices = new List<IService>();
        private bool prepared = false;

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
            coreServices[(int)MessageType.SystemMsg] = new SystemService();
            coreServices[(int)MessageType.TickMsg] = new TickRateService();
            coreServices[(int)MessageType.EventMsg] = new EventService();
            coreServices[(int)MessageType.RoomMsg] = new RoomService();
            coreServices[(int)MessageType.UserMsg] = new UserService();
            coreServices[(int)MessageType.AppMsg] = new AppService();
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
            return coreServices[(int)msg.MsgType].HandleMessage(msg);
        }
    }
}
