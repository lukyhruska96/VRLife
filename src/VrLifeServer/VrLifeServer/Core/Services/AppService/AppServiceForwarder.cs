using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services.RoomService;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Applications;
using VrLifeAPI.Forwarder.Core.Services.AppService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.Core.Applications.DefaultApps;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Database.DbModels;
using VrLifeShared.Core.Services.AppService;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceForwarder : IAppServiceForwarder
    {
        private Dictionary<uint, Dictionary<ulong, IApplicationForwarder>> _appInstances = 
            new Dictionary<uint, Dictionary<ulong, IApplicationForwarder>>();
        private IClosedAPI _api;
        private ILogger _log;
        private AppServiceDataStorage _appServiceData = new AppServiceDataStorage("VrLifeForwarder");
        private AppDataStorage _appStorage;
        private AppDataStorage _clientAppStorage;
        private AppLoader _loader;
        private AppLoader _clientLoader;

        public AppServiceForwarder()
        {
            _appStorage = _appServiceData.AppStorage;
            _clientAppStorage = _appServiceData.GetClientAppStorage();
        }

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
            if(!_appInstances.ContainsKey(roomId.Value) || !_appInstances[roomId.Value].ContainsKey(appId) || _appInstances[roomId.Value][appId] == null)
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
            ulong? userId = _api.Services.User.GetUserIdByClientId(msg.ClientId, true);
            if (!userId.HasValue)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unauthenticated user.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(userId.Value);
            if (!roomId.HasValue)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "User is not connected to any room.");
            }
            AppMsg appMsg = msg.AppMsg;
            switch (appMsg.AppMsgTypeCase)
            {
                case AppMsg.AppMsgTypeOneofCase.AppData:
                    return HandleAppDataMsg(msg, roomId.Value);
                case AppMsg.AppMsgTypeOneofCase.AppPackage:
                    return HandleAppPackageMsg(msg, roomId.Value);
                default:
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown AppMsg type.");
            }
        }

        private MainMessage HandleAppPackageMsg(MainMessage msg, uint roomId)
        {
            AppPackageMsg packageMsg = msg.AppMsg.AppPackage;
            if(packageMsg.AppPackageMsgTypeCase != AppPackageMsg.AppPackageMsgTypeOneofCase.PackageRequest)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not handle this type of AppPackage msg.");
            }
            AppPackageRequest request = packageMsg.PackageRequest;
            switch(request.RequestTypeCase)
            {
                case AppPackageRequest.RequestTypeOneofCase.PackageInfo:
                    return HandlePackageInfoRequest(msg, roomId);
                case AppPackageRequest.RequestTypeOneofCase.PackageList:
                    return HandlePackageList(roomId);
                case AppPackageRequest.RequestTypeOneofCase.DllDataRequest:
                    return HandleDllDataRequest(msg, roomId);
                case AppPackageRequest.RequestTypeOneofCase.ZipDataRequest:
                    return HandleZipDataRequest(msg, roomId);
                default:
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not handle this type of AppPackageRequest msg.");
            }
        }

        private MainMessage HandlePackageList(uint roomId)
        {
            MainMessage msg = new MainMessage();
            msg.AppMsg = new AppMsg();
            msg.AppMsg.AppPackage = new AppPackageMsg();
            msg.AppMsg.AppPackage.PackageList = new AppPackageListMsg();
            msg.AppMsg.AppPackage.PackageList.Packages.AddRange(_appInstances[roomId].Keys.Select(x => new AppPackageListEl { AppId = x }));

            return msg;
        }

        private MainMessage HandleZipDataRequest(MainMessage msg, uint roomId)
        {
            AppPackageDataRequest request = msg.AppMsg.AppPackage.PackageRequest.ZipDataRequest;
            try
            {
                _clientLoader.GetLoadingTask(request.AppId)?.Wait();
            }
            catch(AppLoaderException e)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            string filePath = $"{request.AppId}.zip";
            if (!_clientAppStorage.FileExists(filePath))
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Zip file does not exist.");
            }

            byte[] data = _clientAppStorage.LoadFileChunk(filePath, request.PacketSize, request.PacketId);
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageData = new AppPackageDataMsg();
            response.AppMsg.AppPackage.PackageData.Data = ByteString.CopyFrom(data);
            response.AppMsg.AppPackage.PackageData.Checksum = AppLoader.CalulcateCheckSum(data);
            return response;
        }

        private MainMessage HandleDllDataRequest(MainMessage msg, uint roomId)
        {
            AppPackageDataRequest request = msg.AppMsg.AppPackage.PackageRequest.DllDataRequest;
            try
            {
                _clientLoader.GetLoadingTask(request.AppId)?.Wait();
            }
            catch (AppLoaderException e)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            string filePath = $"{request.AppId}.dll";
            if (!_clientAppStorage.FileExists(filePath))
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "DLL file does not exist.");
            }
            byte[] data = _clientAppStorage.LoadFileChunk(filePath, request.PacketSize, request.PacketId);
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageData = new AppPackageDataMsg();
            response.AppMsg.AppPackage.PackageData.Data = ByteString.CopyFrom(data);
            response.AppMsg.AppPackage.PackageData.Checksum = AppLoader.CalulcateCheckSum(data);
            return response;
        }

        private MainMessage HandlePackageInfoRequest(MainMessage msg, uint roomId)
        {
            ulong appId = msg.AppMsg.AppPackage.PackageRequest.PackageInfo;
            AppPackageInfo packageInfo = _loader.GetPackageInfo(appId);
            if(packageInfo == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unable to recieve AppPackageInfo.");
            }
            LoadPackage(packageInfo, roomId);
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageInfo = packageInfo.ToNetworkModel();

            return response;
        }

        private void LoadPackage(AppPackageInfo packageInfo, uint roomId)
        {
            _clientLoader.PrepareAppPackage(packageInfo, AppPackageDataRequest.Types.PackageDeviceType.Client, (x) => {
                lock (_appInstances[roomId])
                {
                    if(!_appInstances[roomId].ContainsKey(x.ID))
                    {
                        _appInstances[roomId].Add(packageInfo.ID, null);
                    }
                }
            });
            _loader.PrepareAppPackage(packageInfo, AppPackageDataRequest.Types.PackageDeviceType.Forwarder, x => InstantiateApp(x, roomId));
        }

        private void InstantiateApp(AppPackageInfo packageInfo, uint roomId)
        {
            string zipPath = $"{packageInfo.ID}.zip";
            if (_appStorage.FileExists(zipPath))
            {
                MemoryStream memStream = new MemoryStream();
                using (FileStream fs = _appStorage.GetFile(zipPath, FileMode.Open))
                {
                    fs.CopyTo(memStream);
                }
                _appServiceData.GetAppStorage(packageInfo.ToAppInfo()).FromZipStream(memStream);
                memStream.Dispose();
            }
            IApplicationForwarder instance = _loader.LoadApp<IApplicationForwarder>(packageInfo.ID);
            if(instance != null)
            {
                RegisterApp(roomId, instance);
            }
        }

        private MainMessage HandleAppDataMsg(MainMessage msg, uint roomId)
        {
            AppDataMsg appMsg = msg.AppMsg.AppData;
            ulong appId = appMsg.AppId;
            if (!_appInstances.ContainsKey(roomId) || !_appInstances[roomId].ContainsKey(appId) || _appInstances[roomId][appId] == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "No handler could be found for this event.");
            }
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppData = new AppDataMsg();
            response.AppMsg.AppData.AppId = appId;
            byte[] data = appMsg.Data.ToByteArray();
            try
            {
                response.AppMsg.AppData.Data = ByteString.CopyFrom(
                    _appInstances[roomId][appId].HandleMessage(data, data.Length, new MsgContext(msg))
                    );
            }
            catch (Exception e)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            return response;
        }

        public void Init(IClosedAPI api)
        {
            _api = api;
            _log = _api.OpenAPI.CreateLogger(GetType().Name);
            _loader = new AppLoader(_appStorage, _api.OpenAPI.Config.MainServer, _api.OpenAPI.Networking);
            _clientLoader = new AppLoader(_clientAppStorage, _api.OpenAPI.Config.MainServer, _api.OpenAPI.Networking);
            _api.Services.Room.RoomDeleted += OnRoomDeleted;
            _api.Services.Room.RoomCreated += OnRoomCreated;
        }

        private void InitDefaultApps(uint roomId)
        {
            _api.OpenAPI.Apps.DefaultApps[roomId] = new DefaultAppForwarderInstances();
            foreach(IApplicationForwarder app in _api.OpenAPI.Apps.DefaultApps[roomId])
            {
                app.Init(roomId, _api.OpenAPI, _appServiceData.GetAppStorage(app.GetInfo()));
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
            if(_appInstances[roomId].ContainsKey(app.GetInfo().ID) && _appInstances[roomId][app.GetInfo().ID] != null)
            {
                if (_appInstances[roomId][app.GetInfo().ID].GetInfo().Version < app.GetInfo().Version)
                {
                    _appInstances[roomId][app.GetInfo().ID].Dispose();
                    _appInstances[roomId].Remove(app.GetInfo().ID);
                }
                else
                {
                    app.Dispose();
                    throw new AppServiceException("Another instance of this app is already registered.");
                }
            }
            lock(_appInstances[roomId])
            {
                app.Init(roomId, _api.OpenAPI, _appServiceData.GetAppStorage(app.GetInfo()));
                _appInstances[roomId].Remove(app.GetInfo().ID);
                _appInstances[roomId].Add(app.GetInfo().ID, app);
            }
        }

        protected virtual void OnRoomDeleted(uint roomId)
        {
            if(!_appInstances.ContainsKey(roomId))
            {
                return;
            }
            lock(_appInstances[roomId])
            {
                foreach (IApplication app in _appInstances[roomId].Values)
                {
                    app?.Dispose();
                }
            }
            _appInstances.Remove(roomId);
            _api.OpenAPI.Apps.DefaultApps.Remove(roomId);
        }

        protected virtual void OnRoomCreated(IRoom room)
        {
            _appInstances.Add(room.Id, new Dictionary<ulong, IApplicationForwarder>());
            InitDefaultApps(room.Id);
        }
    }
}
