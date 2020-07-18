using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Applications;
using VrLifeServer.Core.Applications.DefaultApps;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceForwarder : IAppServiceForwarder
    {
        private Dictionary<uint, Dictionary<ulong, IApplicationForwarder>> _appInstances = 
            new Dictionary<uint, Dictionary<ulong, IApplicationForwarder>>();
        private ClosedAPI _api;
        private ILogger _log;
        public byte[] HandleEvent(MainMessage msg)
        {
            EventDataMsg eventData = msg.EventMsg.EventDataMsg;
            if(eventData == null)
            {
                throw new EventErrorException("Invalid EventData msg.");
            }
            ulong? userId = _api.Services.User.GetUserIdByClientId(msg.ClientId, true);
            if(!userId.HasValue)
            {
                throw new EventErrorException("Unauthenticated user.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(userId.Value);
            if(!roomId.HasValue)
            {
                throw new EventErrorException("User is not connected to any room.");
            }
            ulong appId = eventData.AppId;
            if(!_appInstances.ContainsKey(roomId.Value) || !_appInstances[roomId.Value].ContainsKey(appId))
            {
                throw new EventErrorException("No handler could be found for this event.");
            }
            try
            {
                return _appInstances[roomId.Value][appId].HandleEvent(eventData, new MsgContext(msg));
            }
            catch(Exception e)
            {
                _log.Error(e);
                throw new EventErrorException(e.Message);
            }
        }

        public MainMessage HandleMessage(MainMessage msg)
        {
            AppMsg appMsg = msg.AppMsg;
            ulong? userId = _api.Services.User.GetUserIdByClientId(msg.ClientId, true);
            if (!userId.HasValue)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unauthenticated user.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(userId.Value);
            if (!roomId.HasValue)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "User is not connected to any room.");
            }
            ulong appId = appMsg.AppId;
            if (!_appInstances.ContainsKey(roomId.Value) || !_appInstances[roomId.Value].ContainsKey(appId))
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "No handler could be found for this event.");
            }
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppId = appId;
            byte[] data = appMsg.Data.ToByteArray();
            try
            {
                response.AppMsg.Data = ByteString.CopyFrom(
                    _appInstances[roomId.Value][appId].HandleMessage(data, data.Length, new MsgContext(msg))
                    );
            }
            catch (Exception e)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            return response;
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
            _log = _api.OpenAPI.CreateLogger(GetType().Name);
            _api.Services.Room.RoomDeleted += OnRoomDeleted;
            _api.Services.Room.RoomCreated += OnRoomCreated;
        }

        private void InitDefaultApps(uint roomId)
        {
            _api.OpenAPI.Apps.DefaultApps[roomId] = new DefaultAppForwarderInstances();
            foreach(IApplicationForwarder app in _api.OpenAPI.Apps.DefaultApps[roomId])
            {
                app.Init(roomId, _api.OpenAPI);
                _appInstances[roomId].Add(app.GetInfo().ID, app);
            }
        }

        public Dictionary<ulong, IApplicationForwarder> GetApplications(uint roomId)
        {
            return _appInstances[roomId];
        }

        public void RegisterApp(uint roomId, IApplicationForwarder app)
        {
            if (app == null)
            {
                throw new AppServiceException("Given app cannot be null.");
            }
            if (!_appInstances.ContainsKey(roomId))
            {
                _appInstances.Add(roomId, new Dictionary<ulong, IApplicationForwarder>());
            }
            if(_appInstances[roomId].ContainsKey(app.GetInfo().ID))
            {
                app.Dispose();
                throw new AppServiceException("Another instance of this app is already registered.");
            }
            app.Init(roomId, _api.OpenAPI);
            _appInstances[roomId].Add(app.GetInfo().ID, app);
        }

        protected virtual void OnRoomDeleted(uint roomId)
        {
            if(!_appInstances.ContainsKey(roomId))
            {
                return;
            }
            foreach(IApplication app in _appInstances[roomId].Values)
            {
                app.Dispose();
            }
            _appInstances.Remove(roomId);
            _api.OpenAPI.Apps.DefaultApps.Remove(roomId);
        }

        protected virtual void OnRoomCreated(Room room)
        {
            _appInstances.Add(room.Id, new Dictionary<ulong, IApplicationForwarder>());
            InitDefaultApps(room.Id);
        }
    }
}
