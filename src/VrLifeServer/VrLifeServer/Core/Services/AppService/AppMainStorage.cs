using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.Core.Applications;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeShared.Core.Services.AppService;

namespace VrLifeServer.Core.Services.AppService
{
    class AppMainStorage
    {
        private string _path;
        private AppServiceProvider _appService;
        public AppMainStorage(AppServiceProvider appService, string path)
        {
            _appService = appService;
            _path = path;
            if(!Directory.Exists(path))
            {
                throw new AppServiceException("App storage directory does not exists.");
            }
        }
        
        // loads all required provider side apps from appRepository to appStorage
        // no instance of this app is created during this process
        public void LoadAll()
        {
            string[] apps = Directory.GetDirectories(_path);
            foreach(string dirPath in apps)
            {
                string strApp = Path.GetFileName(dirPath);
                if (!ulong.TryParse(strApp, out ulong appId))
                {
                    Directory.Delete(dirPath, true);
                    continue;
                }
                LoadApp(appId);
            }
        }

        public byte[] LoadAppFile(ulong appId, string path, uint packetSize, uint packetId)
        {
            string absolutePath = Path.Combine(_path, appId.ToString(), path);
            if(!File.Exists(absolutePath))
            {
                return null;
            }
            byte[] data = new byte[packetSize];
            int readLen;
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(absolutePath, FileMode.Open)))
                {
                    br.BaseStream.Seek(packetSize * packetId, SeekOrigin.Begin);
                    readLen = br.Read(data, 0, (int)packetSize);
                }
            }
            catch(UnauthorizedAccessException)
            {
                return null;
            }
            if(readLen != packetSize)
            {
                byte[] smallerData = new byte[readLen];
                Array.Copy(data, smallerData, readLen);
                return smallerData;
            }
            return data;
        }

        public void LoadApp(ulong appId)
        {
            AppPackageInfo appInfo = GetAppPackageInfo(appId);
            if (appInfo == null)
            {
                Directory.Delete(Path.Combine(_path, appId.ToString()), true);
                return;
            }
            if (_appService.Apps.TryGetValue(appId, out IApplicationProvider app))
            {
                if (app != null && app.GetInfo().Version >= appInfo.Version)
                {
                    return;
                }
            }
            if (appInfo.Provider == null)
            {
                return;
            }
            string dllAbsolutePath = Path.Combine(_path, appId.ToString(), appInfo.Provider.DLLPath);
            string zipAbsolutePath = null;
            if (appInfo.Provider.ZipPath != null) {
                zipAbsolutePath = Path.Combine(_path, appId.ToString(), appInfo.Provider.ZipPath);
            }
            if (!File.Exists(dllAbsolutePath) || (zipAbsolutePath != null && !File.Exists(zipAbsolutePath)))
            {
                Directory.Delete(Path.Combine(_path, appId.ToString()));
                return;
            }
            string appStoragePath = Path.Combine(_appService.AppDataStorage.APPDATA_FOLDER, 
                _appService.AppDataStorage.APPSTORAGE_FOLDER_NAME);
            try
            {
                File.Copy(dllAbsolutePath, Path.Combine(appStoragePath, $"{appId}.dll"), true);
                File.WriteAllText(Path.Combine(appStoragePath, $"{appId}.VERSION"), appInfo.Version.ToString());
            }
            catch(UnauthorizedAccessException)
            {
                Directory.Delete(Path.Combine(_path, appId.ToString()), true);
                return;
            }
            AppDataStorage storage = null;
            if (zipAbsolutePath != null)
            {
                storage = _appService.AppDataStorage.GetAppStorage(appInfo.ToAppInfo());
                try
                {
                    storage.FromZipFile(zipAbsolutePath);
                }
                catch(Exception)
                {
                    Directory.Delete(Path.Combine(_path, appId.ToString()), true);
                    return;
                }
            }
        }

        public List<AppPackageInfo> ListAppPackages()
        {
            return Directory.GetDirectories(_path)
                .Select(x => Path.GetFileName(x))
                .Where(x => ulong.TryParse(x, out _))
                .Select(x => ulong.Parse(x))
                .Select(x => GetAppPackageInfo(x))
                .Where(x => x != null)
                .ToList();
        }

        public AppPackageInfo GetAppPackageInfo(ulong appId)
        {
            string path = Path.Combine(_path, appId.ToString());
            if (!Directory.Exists(path))
            {
                return null;
            }
            string appJsonPath = Path.Combine(path, "app.json");
            if (!File.Exists(appJsonPath))
            {
                return null;
            }
            string content;
            try
            {
                content = File.ReadAllText(appJsonPath);
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            JObject jContent;
            try
            {
                jContent = JObject.Parse(content);
            }
            catch (JsonReaderException)
            {
                return null;
            }
            try
            {
                return new AppPackageInfo(jContent);
            }
            catch (AppPackageInfoException)
            {
                return null;
            }
        }
    }
}
