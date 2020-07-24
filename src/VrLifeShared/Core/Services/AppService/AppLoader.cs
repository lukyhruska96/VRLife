using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.Core.Applications;
using static VrLifeAPI.Networking.NetworkingModels.AppPackageDataRequest.Types;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppLoader
    {
        private const int PACKET_SIZE = 4096;
        private AppDataStorage _appStorage;
        private IPEndPoint _address;
        private IUDPNetworking<MainMessage> _networking;
        private Dictionary<ulong, Task> _loadingLock = new Dictionary<ulong, Task>();
        private Dictionary<ulong, AppVersion> _localApps = new Dictionary<ulong, AppVersion>();
        public AppLoader(AppDataStorage appStorage, IPEndPoint address, IUDPNetworking<MainMessage> networking)
        {
            _appStorage = appStorage;
            _address = address;
            _networking = networking;
            InitLocalApps();
        }

        private void InitLocalApps()
        {
            var appList = _appStorage.ListFiles().Where(x => x.Contains(".dll"))
                .Select(x => x.Replace(".dll", ""))
                .Where(x => ulong.TryParse(x, out _));
            foreach(string appFile in appList)
            {
                ulong appId = ulong.Parse(appFile);
                string versionFile = $"{appId}.VERSION";
                if (!_appStorage.FileExists(versionFile))
                {
                    _appStorage.RemoveFile($"{appId}.dll");
                    _appStorage.RemoveFile($"{appId}.zip");
                    continue;
                }

                string version;
                using (StreamReader reader = new StreamReader(_appStorage.GetFile(versionFile, FileMode.Open)))
                {
                    version = reader.ReadToEnd();
                }
                string[] splitVersion = version.Split('.');
                if (splitVersion.Length != 3)
                {
                    _appStorage.RemoveFile($"{appId}.dll");
                    _appStorage.RemoveFile($"{appId}.zip");
                    continue;
                }
                int[] intVersion = new int[3];
                intVersion[0] = int.Parse(splitVersion[0]);
                intVersion[1] = int.Parse(splitVersion[1]);
                intVersion[2] = int.Parse(splitVersion[2]);
                AppVersion appVersion = new AppVersion(intVersion);
                _localApps.Add(appId, appVersion);
            }
        }

        public List<T> LoadAll<T>()
        {
            return _appStorage.ListFiles()
                .Where(x => x.Contains(".dll"))
                .Select(x => x.Replace(".dll", ""))
                .Where(x => ulong.TryParse(x, out _))
                .Select(x => LoadApp<T>(ulong.Parse(x)))
                .Where(x => x != null)
                .ToList<T>();
        }

        public Task PrepareAppPackage(AppPackageInfo packageInfo, PackageDeviceType type, Action<AppPackageInfo> appLoadedCallback = null)
        {
            if(packageInfo.Forwarder == null && type == PackageDeviceType.Forwarder)
            {
                return null;
            }
            if(packageInfo.Client == null && type == PackageDeviceType.Client)
            {
                return null;
            }
            if(!_loadingLock.TryGetValue(packageInfo.ID, out Task t))
            {
                if(CheckLocalStorage(packageInfo.ID, packageInfo.Version))
                {
                    List<Task> deps = new List<Task>();
                    if(packageInfo.Dependencies != null)
                    {
                        foreach (AppPackageInfo dep in packageInfo.Dependencies)
                        {
                            AppPackageInfo pkgInfo = GetPackageInfo(dep.ID);
                            if (pkgInfo != null)
                            {
                                Task ttt = PrepareAppPackage(pkgInfo, type, appLoadedCallback);
                                if (ttt != null)
                                {
                                    deps.Add(ttt);
                                }
                            }
                        }
                    }
                    if(deps.Count != 0)
                    {
                        return Task.Run(() =>
                        {
                            deps.ForEach(x => x.Wait());
                            appLoadedCallback(packageInfo);
                        });
                    }
                    appLoadedCallback(packageInfo);
                    return null;
                }
                Task tt = Task.Run(() =>
                {
                    PrepareAppPackageSync(packageInfo, type, appLoadedCallback);
                    lock(_loadingLock)
                    {
                        _loadingLock.Remove(packageInfo.ID);
                    }
                });
                _loadingLock.Add(packageInfo.ID, tt);
                return tt;
            }
            return t;
        }

        public AppPackageInfo GetPackageInfo(ulong appId)
        {
            MainMessage req = new MainMessage();
            req.AppMsg = new AppMsg();
            req.AppMsg.AppPackage = new AppPackageMsg();
            req.AppMsg.AppPackage.PackageRequest = new AppPackageRequest();
            req.AppMsg.AppPackage.PackageRequest.PackageInfo = appId;
            MainMessage response = _networking.Send(req, _address);
            if (VrLifeAPI.Common.Core.Services.ServiceUtils.IsError(response))
            {
                return null;
            }
            AppPackageInfoMsg packageInfo = response.AppMsg.AppPackage.PackageInfo;
            if(packageInfo == null)
            {
                return null;
            }
            return new AppPackageInfo(packageInfo);
        }

        private bool CheckLocalStorage(ulong appId, AppVersion version) 
        {
            return _localApps.TryGetValue(appId, out AppVersion ver) && ver >= version;
        }

        public Task GetLoadingTask(ulong appId)
        {
            if(_loadingLock.TryGetValue(appId, out Task t))
            {
                return t;
            }
            if(_localApps.TryGetValue(appId, out AppVersion version))
            {
                return null;
            }

            throw new AppLoaderException("This app has never been loaded.");
        }

        private void PrepareAppPackageSync(AppPackageInfo packageInfo, PackageDeviceType type, Action<AppPackageInfo> appLoadedCallback = null)
        {
            if(packageInfo.Dependencies != null)
            {
                List<Task> dependencies = new List<Task>();
                foreach(AppPackageInfo pkg in packageInfo.Dependencies)
                {
                    AppPackageInfo pkgInfo = GetPackageInfo(pkg.ID);
                    Task t = PrepareAppPackage(pkgInfo, type, appLoadedCallback);
                    if(t != null)
                    {
                        dependencies.Add(t);
                    }
                }
                dependencies.ForEach(x => x.Wait());
            }

            if(!DownloadDllPkg(packageInfo, type))
            {
                _appStorage.RemoveFile($"{packageInfo.ID}.dll");
                throw new AppLoaderException("Could not download all app files.");
            }
            if (!DownloadZipPkg(packageInfo, type))
            {
                _appStorage.RemoveFile($"{packageInfo.ID}.zip");
                throw new AppLoaderException("Could not download all app files.");
            }
            using (StreamWriter sw = new StreamWriter(_appStorage.GetFile($"{packageInfo.ID}.VERSION", FileMode.OpenOrCreate)))
            {
                sw.Write(packageInfo.Version.ToString());
            }
            if(_localApps.ContainsKey(packageInfo.ID))
            {
                _localApps.Remove(packageInfo.ID);
            }
            _localApps.Add(packageInfo.ID, packageInfo.Version);
            appLoadedCallback?.Invoke(packageInfo);
        }

        private bool DownloadDllPkg(AppPackageInfo pkg, PackageDeviceType type)
        {
            IAppPackageItemInfo pkgItem;
            switch(type)
            {
                case PackageDeviceType.Forwarder:
                    pkgItem = pkg.Forwarder;
                    break;
                default:
                    pkgItem = pkg.Client;
                    break;
            }
            if(pkgItem == null || pkgItem.DLLPath == null)
            {
                return true;
            }
            uint retLen = PACKET_SIZE;
            uint i = 0;
            using(FileStream fs = _appStorage.GetFile($"{pkg.ID}.dll", FileMode.OpenOrCreate))
            {
                while(retLen == PACKET_SIZE)
                {
                    MainMessage request = new MainMessage();
                    request.AppMsg = new AppMsg();
                    request.AppMsg.AppPackage = new AppPackageMsg();
                    request.AppMsg.AppPackage.PackageRequest = new AppPackageRequest();
                    request.AppMsg.AppPackage.PackageRequest.DllDataRequest = new AppPackageDataRequest();
                    request.AppMsg.AppPackage.PackageRequest.DllDataRequest.AppId = pkg.ID;
                    request.AppMsg.AppPackage.PackageRequest.DllDataRequest.PackageType = type;
                    request.AppMsg.AppPackage.PackageRequest.DllDataRequest.PacketSize = PACKET_SIZE;
                    request.AppMsg.AppPackage.PackageRequest.DllDataRequest.PacketId = i;
                    MainMessage response = _networking.Send(request, _address);
                    if(VrLifeAPI.Common.Core.Services.ServiceUtils.IsError(response))
                    {
                        return false;
                    }
                    AppPackageDataMsg data = response.AppMsg.AppPackage.PackageData;
                    if(data == null)
                    {
                        return false;
                    }
                    byte[] dataArr = data.Data.ToArray();
                    if (CalulcateCheckSum(dataArr) != data.Checksum)
                    {
                        continue;
                    }
                    retLen = (uint)dataArr.Length;
                    fs.Write(dataArr, 0, dataArr.Length);
                    ++i;
                }
            }
            return true;
        }

        private bool DownloadZipPkg(AppPackageInfo pkg, PackageDeviceType type)
        {
            IAppPackageItemInfo pkgItem;
            switch (type)
            {
                case PackageDeviceType.Forwarder:
                    pkgItem = pkg.Forwarder;
                    break;
                default:
                    pkgItem = pkg.Client;
                    break;
            }
            if (pkgItem == null || pkgItem.ZipPath == null)
            {
                return true;
            }
            uint retLen = PACKET_SIZE;
            uint i = 0;
            using (FileStream fs = _appStorage.GetFile($"{pkg.ID}.zip", FileMode.OpenOrCreate))
            { 
                while (retLen == PACKET_SIZE)
                {
                    MainMessage request = new MainMessage();
                    request.AppMsg = new AppMsg();
                    request.AppMsg.AppPackage = new AppPackageMsg();
                    request.AppMsg.AppPackage.PackageRequest = new AppPackageRequest();
                    request.AppMsg.AppPackage.PackageRequest.ZipDataRequest = new AppPackageDataRequest();
                    request.AppMsg.AppPackage.PackageRequest.ZipDataRequest.AppId = pkg.ID;
                    request.AppMsg.AppPackage.PackageRequest.ZipDataRequest.PackageType = type;
                    request.AppMsg.AppPackage.PackageRequest.ZipDataRequest.PacketSize = PACKET_SIZE;
                    request.AppMsg.AppPackage.PackageRequest.ZipDataRequest.PacketId = i;
                    MainMessage response = _networking.Send(request, _address);
                    if (VrLifeAPI.Common.Core.Services.ServiceUtils.IsError(response))
                    {
                        return false;
                    }
                    AppPackageDataMsg data = response.AppMsg.AppPackage.PackageData;
                    if (data == null)
                    {
                        return false;
                    }
                    byte[] dataArr = data.Data.ToArray();
                    if (CalulcateCheckSum(dataArr) != data.Checksum)
                    {
                        continue;
                    }
                    retLen = (uint)dataArr.Length;
                    fs.Write(dataArr, 0, dataArr.Length);
                    ++i;
                }
            }
            return true;
        }
        public static ulong CalulcateCheckSum(byte[] data)
        {
            ulong sum = 0;
            unchecked
            {
                foreach (byte b in data)
                {
                    sum += b;
                }
            }
            return sum;
        }

        public T LoadApp<T>(ulong appId)
        {
            string dllFileName = $"{appId}.dll";
            if (!_appStorage.FileExists(dllFileName))
            {
                throw new AppLoaderException($"File {appId}.dll does not exists.");
            }
            byte[] dllData;
            using (FileStream fs = _appStorage.GetFile(dllFileName, FileMode.Open))
            {
                dllData = new byte[fs.Length];
                fs.Read(dllData, 0, dllData.Length);
            }
            try
            {
                T instance = AppLoader.InstantiateApp<T>(dllData);
                return instance;
            }
            catch (Exception)
            {
                _appStorage.RemoveFile(dllFileName);
                return default;
            }
        }

        public static T InstantiateApp<T>(byte[] data)
        {
            Assembly assembly = Assembly.Load(data);
            Type t = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(T))).FirstOrDefault();
            if (t == null)
            {
                return default;
            }
            return (T)Activator.CreateInstance(t);
        }
    }
}
