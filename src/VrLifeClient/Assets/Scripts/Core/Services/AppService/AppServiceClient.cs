using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.AppService;
using Assets.Scripts.Core.Services.EventService;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Services.AppService;
using VrLifeShared.Core.Services.EventService;

namespace VrLifeClient.Core.Services.AppService
{
    class AppServiceClient : IAppServiceClient
    {
        private IClosedAPI _api;
        public Dictionary<ulong, IApplication> AllApps { get; set; } = new Dictionary<ulong, IApplication>();
        public List<IMenuApp> MenuApps { get; private set; } = new List<IMenuApp>();
        public List<IBackgroundApp> BackgroundApps { get; private set; } = new List<IBackgroundApp>();
        public List<IGlobalAPp> GlobalApps { get; private set; } = new List<IGlobalAPp>();
        public List<IObjectApp> ObjectApps { get; private set; } = new List<IObjectApp>();
        public Dictionary<ulong, List<IObjectAppInstance>> ObjectAppInstances { get; private set; } = new Dictionary<ulong, List<IObjectAppInstance>>();

        public event Action<AppInfo> AddedNewApp;

        public event Action<IObjectAppInstance> AddedNewObjectAppInstance;

        private AppServiceDataStorage _dataStorage = new AppServiceDataStorage("VrLifeClient");
        private AppDataStorage _appStorage;
        private AppLoader _loader;
        private Task _appPackageInterval = null;
        private bool _stopAppPackageInterval = false;
        private const int APP_PKG_INTERVAL_MS = 5000;

        public AppServiceClient()
        {
            _appStorage = _dataStorage.AppStorage;
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._api.Services.Room.RoomExited += Reset;
            this._api.Services.Room.RoomEntered += OnRoomEnter;
        }

        public void RegisterApp(IApplication app)
        {
            if(app == null)
            {
                throw new AppServiceException("Given app cannot be null.");
            }
            AppInfo info = app.GetInfo();
            if(AllApps.ContainsKey(info.ID))
            {
                app.Dispose();
                throw new AppServiceException("Another instance of this app is already registered.");
            }
            try
            {
                switch (info.Type)
                {
                    case AppType.APP_BACKGROUND:
                        RegisterBackgroundApp((IBackgroundApp)app);
                        break;
                    case AppType.APP_MENU:
                        RegisterMenuApp((IMenuApp)app);
                        break;
                    case AppType.APP_GLOBAL:
                        RegisterGlobalApp((IGlobalAPp)app);
                        break;
                    case AppType.APP_OBJECT:
                        RegisterObjectApp((IObjectApp)app);
                        break;
                    default:
                        throw new AppServiceException("Unknown application type.");
                }
            }
            catch(Exception e)
            {
                app.Dispose();
                return;
            }
            AllApps.Add(info.ID, app);
        }

        public IServiceCallback<List<IAppPackageInfo>> ListAppPackages()
        {
            return new ServiceCallback<List<IAppPackageInfo>>(() =>
            {
                MainMessage msg = new MainMessage();
                msg.AppMsg = new AppMsg();
                msg.AppMsg.AppPackage = new AppPackageMsg();
                msg.AppMsg.AppPackage.PackageRequest = new AppPackageRequest();
                msg.AppMsg.AppPackage.PackageRequest.PackageList = true;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                if(SystemServiceClient.IsErrorMsg(response))
                {
                    throw new AppServiceException(response.SystemMsg.ErrorMsg.ErrorMsg_);
                }
                AppPackageListMsg listMsg = response.AppMsg.AppPackage.PackageList;
                if(listMsg == null)
                {
                    throw new AppServiceException("Unknown response.");
                }
                return listMsg.Packages.Select(x => new AppPackageInfo(x)).ToList<IAppPackageInfo>();
            });
        }

        public IServiceCallback<bool> LoadApp(ulong appId)
        {
            return new ServiceCallback<bool>(() =>
            {
                AppPackageInfo packageInfo = _loader.GetPackageInfo(appId);
                if (packageInfo == null)
                {
                    throw new AppServiceException("Could find app with this ID.");
                }
                _loader.PrepareAppPackage(packageInfo, AppPackageDataRequest.Types.PackageDeviceType.Client, OnPackageLoaded);
                return true;
            });
        }

        private void OnPackageLoaded(AppPackageInfo pkg)
        {
            string zipPath = $"{pkg.ID}.zip";
            if (_appStorage.FileExists(zipPath))
            {
                MemoryStream memStream = new MemoryStream();
                using (FileStream fs = _appStorage.GetFile(zipPath, FileMode.Open))
                {
                    fs.CopyTo(memStream);
                }
                _dataStorage.GetAppStorage(pkg.ToAppInfo()).FromZipStream(memStream);
                memStream.Dispose();
            }
            IApplication instance = _loader.LoadApp<IApplication>(pkg.ID);
            if (instance != null)
            {
                RegisterApp(instance);
                try
                {
                    AddedNewApp?.Invoke(instance.GetInfo());
                }
                catch (Exception e) 
                {

                }
            }
        }

        private void InitDefaultApps()
        {
            foreach(IApplication app in _api.OpenAPI.DefaultApps)
            {
                RegisterApp(app);
            }
        }

        private void RegisterMenuApp(IMenuApp app)
        {
            app.Init(_api.OpenAPI, _api.MenuAPI, _api.HUDAPI);
            MenuApps.Add(app);
        }

        private void RegisterBackgroundApp(IBackgroundApp app)
        {
            app.Init(_api.OpenAPI);
            BackgroundApps.Add(app);
        }

        private void RegisterGlobalApp(IGlobalAPp app)
        {
            app.Init(_api.OpenAPI, _api.GlobalAPI);
            GlobalApps.Add(app);
        }

        private void RegisterObjectApp(IObjectApp app)
        {
            app.Init(_api.OpenAPI, _api.ObjectAPI);
            ObjectApps.Add(app);
        }

        private void Reset()
        {
            if(_appPackageInterval != null && _appPackageInterval.Status == TaskStatus.Running)
            {
                _stopAppPackageInterval = true;
                _appPackageInterval.Wait();
                _stopAppPackageInterval = false;
            }
            foreach(IApplication app in AllApps.Values)
            {
                app.Dispose();
            }
            AllApps.Clear();
            MenuApps.Clear();
            GlobalApps.Clear();
            BackgroundApps.Clear();
            ObjectApps.Clear();
            foreach(var appList in ObjectAppInstances.Values)
            {
                appList.ForEach(x => x.Dispose());
            }
            ObjectAppInstances.Clear();
        }

        private void OnRoomEnter()
        {
            InitDefaultApps();
            _loader = new AppLoader(_appStorage, _api.Services.Room.ForwarderAddress, _api.OpenAPI.Networking);
            InitRoomAppsInterval();
        }

        private void InitRoomAppsInterval()
        {
            _appPackageInterval = new Task(() =>
            {
                while(!_stopAppPackageInterval)
                {
                    InitRoomApps();
                    for(int i = 0; i < 10; ++i)
                    {
                        if(_stopAppPackageInterval)
                        {
                            return;
                        }
                        Thread.Sleep(APP_PKG_INTERVAL_MS / 10);
                    }
                }
            });
            _appPackageInterval.Start();
        }

        private void InitRoomApps()
        {
            MainMessage msg = new MainMessage();
            msg.AppMsg = new AppMsg();
            msg.AppMsg.AppPackage = new AppPackageMsg();
            msg.AppMsg.AppPackage.PackageRequest = new AppPackageRequest();
            msg.AppMsg.AppPackage.PackageRequest.PackageList = true;
            MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.Services.Room.ForwarderAddress);
            if(SystemServiceClient.IsErrorMsg(response))
            {
                return;
            }
            AppPackageListMsg listMsg = response.AppMsg.AppPackage.PackageList;
            if(listMsg == null)
            {
                return;
            }
            foreach(AppPackageListEl el in listMsg.Packages)
            {
                if(!AllApps.ContainsKey(el.AppId))
                {
                    LoadApp(el.AppId).Exec();
                }
            }
        }

        public IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient)
        {
            return new ServiceCallback<byte[]>(() =>
            {
                MainMessage mainMsg = new MainMessage();
                mainMsg.AppMsg = new AppMsg();
                mainMsg.AppMsg.AppData = new AppDataMsg();
                mainMsg.AppMsg.AppData.AppId = app.ID;
                mainMsg.AppMsg.AppData.Data = ByteString.CopyFrom(data);
                MainMessage response = null;
                switch(recipient)
                {
                    case AppMsgRecipient.FORWARDER:
                        response = _api.OpenAPI.Networking.Send(mainMsg, _api.Services.Room.ForwarderAddress);
                        break;
                    case AppMsgRecipient.PROVIDER:
                        response = _api.OpenAPI.Networking.Send(mainMsg, _api.OpenAPI.Config.MainServer);
                        break;
                }
                if(response == null)
                {
                    throw new AppServiceException("Unknown response.");
                }
                if(SystemServiceClient.IsErrorMsg(response))
                {
                    throw new AppServiceException(response.SystemMsg.ErrorMsg.ErrorMsg_);
                }
                AppMsg appMsg = response.AppMsg;
                if(appMsg == null)
                {
                    throw new AppServiceException("Unknown response.");
                }
                return appMsg.AppData.Data.ToByteArray();
            });
        }

        public void CreateObjectAppInstance(ulong appId, Vector3 center)
        {
            if(!AllApps.TryGetValue(appId, out IApplication app))
            {
                throw new AppServiceException("Could not find app with this ID.");
            }
            if(app.GetInfo().Type != AppType.APP_OBJECT)
            {
                throw new AppServiceException("Not a object app.");
            }
            CreateObjectAppInstance((IObjectApp)app, center);
        }

        public void CreateObjectAppInstance(IObjectApp app, Vector3 center)
        {
            ulong appId = app.GetInfo().ID;
            if (!ObjectAppInstances.TryGetValue(appId, out var list))
            {
                list = new List<IObjectAppInstance>();
                ObjectAppInstances.Add(appId, list);
            }
            CreateObjectAppInstance(app, (ulong)list.Count, center);
        }

        public void CreateObjectAppInstance(ulong appId, ulong appInstanceId, Vector3 center)
        {
            if (!AllApps.TryGetValue(appId, out IApplication app))
            {
                throw new AppServiceException("Could not find app with this ID.");
            }
            if (app.GetInfo().Type != AppType.APP_OBJECT)
            {
                throw new AppServiceException("Not a object app.");
            }
            CreateObjectAppInstance((IObjectApp)app, appInstanceId, center);
        }

        public void CreateObjectAppInstance(IObjectApp app, ulong appInstanceId, Vector3 center)
        {
            ulong appId = app.GetInfo().ID;
            if (!ObjectAppInstances.TryGetValue(appId, out var list))
            {
                list = new List<IObjectAppInstance>();
                ObjectAppInstances.Add(appId, list);
            }
            if (list.Count > (int)appInstanceId && list[(int)appInstanceId] != null)
            {
                return;
            }
            while(list.Count <= (int)appInstanceId)
            {
                list.Add(null);
            }
            IObjectAppInstance instance = app.CreateInstance(appInstanceId, center);
            if (instance == null)
            {
                throw new AppServiceException("Could not create an instance of the app.");
            }
            list[(int)appInstanceId] = instance;
            EventDataMsg eventData = new EventDataMsg();
            eventData.ObjectValue = new GameObject();
            eventData.EventType = (uint)EventType.OBJECT_STATE;
            eventData.ObjectValue.AppId = appId;
            eventData.ObjectValue.AppInstanceId = appInstanceId;
            eventData.ObjectValue.Center = center.ToCoord();
            try
            {
                _api.Services.Event.SendEvent(eventData, EventRecipient.FORWARDER).Wait();
                AddedNewObjectAppInstance?.Invoke(instance);
            }
            catch (EventServiceException e)
            {
                instance.Dispose();
                ObjectAppInstances[appId][(int)appInstanceId] = null;
            }
        }
    }
}
