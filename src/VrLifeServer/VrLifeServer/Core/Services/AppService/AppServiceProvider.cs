using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Applications;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeServer.Core.Services.EventService;
using VrLifeShared.Core.Services.AppService;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceProvider : IAppServiceProvider
    {
        private IClosedAPI _api;
        private ILogger _log;
        private Dictionary<ulong, IApplicationProvider> _appInstances = 
            new Dictionary<ulong, IApplicationProvider>();
        public AppServiceDataStorage AppDataStorage { get; private set; } = new AppServiceDataStorage("VrLifeProvider");
        private AppMainStorage _appMainStorage;
        public AppLoader AppLoader { get; private set; }
        public ReadOnlyDictionary<ulong, IApplicationProvider> Apps { get; }

        public AppServiceProvider() 
        {
            Apps = new ReadOnlyDictionary<ulong, IApplicationProvider>(_appInstances);
            AppLoader = new AppLoader(AppDataStorage.AppStorage, null, null);
        }

        public byte[] HandleEvent(MainMessage msg)
        {
            EventDataMsg eventData = msg.EventMsg.EventDataMsg;
            if (eventData == null)
            {
                throw new EventErrorException("Invalid EventData msg.");
            }
            ulong appId = eventData.AppId;
            if (!_appInstances.ContainsKey(appId))
            {
                throw new EventErrorException("No handler could be found for this event.");
            }
            try
            {
                return _appInstances[appId].HandleEvent(eventData, new MsgContext(msg));
            }
            catch(Exception e)
            {
                throw new EventErrorException($"{e.GetType().Name}: {e.Message}");
            }
        }

        public MainMessage HandleMessage(MainMessage msg)
        {
            switch(msg.AppMsg.AppMsgTypeCase)
            {
                case AppMsg.AppMsgTypeOneofCase.AppData:
                    return HandleAppDataMsg(msg);
                case AppMsg.AppMsgTypeOneofCase.AppPackage:
                    return HandleAppPackageMsg(msg);
                default:
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown AppPackage request type.");
            }
        }

        private MainMessage HandleAppPackageMsg(MainMessage msg)
        {
            AppPackageRequest appPackageRequest = msg.AppMsg.AppPackage.PackageRequest;
            if(appPackageRequest == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not handle this type of AppPackage msg.");
            }
            switch(appPackageRequest.RequestTypeCase)
            {
                case AppPackageRequest.RequestTypeOneofCase.PackageInfo:
                    return HandleAppPackgeInfoRequest(msg);
                case AppPackageRequest.RequestTypeOneofCase.PackageList:
                    return HandleListAppPackages(msg);
                case AppPackageRequest.RequestTypeOneofCase.DllDataRequest:
                    return HandleDllDataRequest(msg);
                case AppPackageRequest.RequestTypeOneofCase.ZipDataRequest:
                    return HandleZipDataRequest(msg);
                default:
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not handle this type of AppPackageRequest msg.");
            }
        }

        // because all packages are in local storage there is no problem in waiting for
        // initialization of provider's instance of this app and its dependencies
        private MainMessage HandleAppPackgeInfoRequest(MainMessage msg)
        {
            ulong appId = msg.AppMsg.AppPackage.PackageRequest.PackageInfo;
            AppPackageInfo info = _appMainStorage.GetAppPackageInfo(appId);
            if(info == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App with this ID could not be found.");
            }
            if(!CheckPackageInstance(info))
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App dependencies problem.");
            }
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageInfo = info.ToNetworkModel();
            return response;
        }

        private bool CheckPackageInstance(AppPackageInfo app)
        {
            if(app.Provider == null)
            {
                return true;
            }
            // loading twice from dictionary is still cheaper then loading json from file
            // and this line is needed for root level of recursion
            if (_appInstances.TryGetValue(app.ID, out IApplicationProvider localApp) && localApp.GetInfo().Version >= app.Version)
            {
                return true;
            }
            if (app.Dependencies != null)
            {
                foreach(AppPackageInfo dep in app.Dependencies)
                {
                    if(_appInstances.TryGetValue(dep.ID, out IApplicationProvider depApp) && depApp.GetInfo().Version >= dep.Version)
                    {
                        continue;
                    }
                    AppPackageInfo depInfo = _appMainStorage.GetAppPackageInfo(dep.ID);
                    if(depInfo == null)
                    {
                        return false;
                    }
                    if(!CheckPackageInstance(depInfo))
                    {
                        return false;
                    }
                }
            }
            _appMainStorage.LoadApp(app.ID);
            IApplicationProvider instance = AppLoader.LoadApp<IApplicationProvider>(app.ID);
            if(instance == null)
            {
                return false;
            }
            RegisterApp(instance);
            return true;
        }

        // because list structure is not complete AppPackageInfo object,
        // client/forwarder must ask for full info separately, so there is no need to
        // preinitialize every app in this list
        private MainMessage HandleListAppPackages(MainMessage msg)
        {
            List<AppPackageInfo> packages = _appMainStorage.ListAppPackages();
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageList = new AppPackageListMsg();
            response.AppMsg.AppPackage.PackageList.Packages.AddRange(packages.Select(x => x.ToNetworkListEl()));
            return response;
        }

        private MainMessage HandleDllDataRequest(MainMessage msg)
        {
            AppPackageDataRequest request = msg.AppMsg.AppPackage.PackageRequest.DllDataRequest;
            ulong appId = request.AppId;
            AppPackageInfo info = _appMainStorage.GetAppPackageInfo(appId);
            if (info == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App with this ID could not be found.");
            }
            string path;
            if(request.PackageType == AppPackageDataRequest.Types.PackageDeviceType.Client)
            {
                if(info.Client == null || info.Client.DLLPath == null)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App does not contains this type of data.");
                }
                path = info.Client.DLLPath;
            }
            else
            {
                if (info.Forwarder == null || info.Forwarder.DLLPath == null)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App does not contains this type of data.");
                }
                path = info.Forwarder.DLLPath;
            }
            byte[] data = _appMainStorage.LoadAppFile(appId, path, request.PacketSize, request.PacketId);
            if (data == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not find this type of data.");
            }
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageData = new AppPackageDataMsg();
            response.AppMsg.AppPackage.PackageData.Data = ByteString.CopyFrom(data);
            response.AppMsg.AppPackage.PackageData.Checksum = AppLoader.CalulcateCheckSum(data);
            return response;
        }

        private MainMessage HandleZipDataRequest(MainMessage msg)
        {
            AppPackageDataRequest request = msg.AppMsg.AppPackage.PackageRequest.ZipDataRequest;
            ulong appId = request.AppId;
            AppPackageInfo info = _appMainStorage.GetAppPackageInfo(appId);
            if (info == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App with this ID could not be found.");
            }
            string path;
            if (request.PackageType == AppPackageDataRequest.Types.PackageDeviceType.Client)
            {
                if (info.Client == null || info.Client.ZipPath == null)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App does not contains this type of data.");
                }
                path = info.Client.ZipPath;
            }
            else
            {
                if (info.Forwarder == null || info.Forwarder.ZipPath == null)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "App does not contains this type of data.");
                }
                path = info.Forwarder.ZipPath;
            }
            byte[] data = _appMainStorage.LoadAppFile(appId, path, request.PacketSize, request.PacketId);
            if (data == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Could not find this type of data.");
            }
            MainMessage response = new MainMessage();
            response.AppMsg = new AppMsg();
            response.AppMsg.AppPackage = new AppPackageMsg();
            response.AppMsg.AppPackage.PackageData = new AppPackageDataMsg();
            response.AppMsg.AppPackage.PackageData.Data = ByteString.CopyFrom(data);
            response.AppMsg.AppPackage.PackageData.Checksum = AppLoader.CalulcateCheckSum(data);
            return response;
        }

        private MainMessage HandleAppDataMsg(MainMessage msg)
        {
            AppDataMsg appMsg = msg.AppMsg.AppData;
            ulong appId = appMsg.AppId;
            if (!_appInstances.ContainsKey(appId))
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
                byte[] responseData = _appInstances[appId].HandleMessage(data, data.Length, new MsgContext(msg));
                if (responseData != null)
                {
                    response.AppMsg.AppData.Data = ByteString.CopyFrom(responseData);
                }
            }
            catch (Exception e)
            {
                _log.Error(e);
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            return response;
        }

        public void Init(IClosedAPI api)
        {
            _api = api;
            _log = _api.OpenAPI.CreateLogger(GetType().Name);
            _appMainStorage = new AppMainStorage(this, api.OpenAPI.Config.AppStoragePath);
            InitDefaultApps();
            // load all new apps to app storage
            _appMainStorage.LoadAll();
            // initialize all apps in app storage
            RegisterMultipleApps(AppLoader.LoadAll<IApplicationProvider>());
        }

        public void InitDefaultApps()
        {
            foreach(IApplicationProvider app in _api.OpenAPI.Apps)
            {
                app.Init(_api.OpenAPI, new AppDataService(app.GetInfo().ID), AppDataStorage.GetAppStorage(app.GetInfo()));
                _appInstances.Add(app.GetInfo().ID, app);
            }
        }

        public void RegisterApp(IApplicationProvider app)
        {
            if (app == null)
            {
                throw new AppServiceException("Given app cannot be null.");
            }
            if (_appInstances.ContainsKey(app.GetInfo().ID))
            {
                if(_appInstances[app.GetInfo().ID].GetInfo().Version < app.GetInfo().Version)
                {
                    _appInstances[app.GetInfo().ID].Dispose();
                    _appInstances.Remove(app.GetInfo().ID);
                }
                else
                {
                    app.Dispose();
                    throw new AppServiceException("Another instance of this app is already registered.");
                }
            }
            _appInstances.Add(app.GetInfo().ID, app);
            try
            {
                app.Init(_api.OpenAPI, new AppDataService(app.GetInfo().ID), AppDataStorage.GetAppStorage(app.GetInfo()));
            }
            catch(Exception e)
            {
                _log.Error(e);
                _appInstances.Remove(app.GetInfo().ID);
                app.Dispose();
            }
        }

        // because some of those apps might be dependent on another, we must first add all apps into instance list and then initialize them
        public void RegisterMultipleApps(List<IApplicationProvider> apps)
        {
            apps = apps.Where(x => x != null).Where(x => !_appInstances.ContainsKey(x.GetInfo().ID)).ToList();
            apps.ForEach(x => _appInstances.Add(x.GetInfo().ID, x));
            foreach(IApplicationProvider app in apps)
            {
                try
                {
                    app.Init(_api.OpenAPI, new AppDataService(app.GetInfo().ID), AppDataStorage.GetAppStorage(app.GetInfo()));
                }
                catch(Exception)
                {
                    _appInstances.Remove(app.GetInfo().ID);
                    app.Dispose();
                }
            }
        }
    }
}
