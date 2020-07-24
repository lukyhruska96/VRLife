using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VrLifeAPI;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppServiceDataStorage
    {
        public readonly string APPDATA_FOLDER;
        public readonly string APPSTORAGE_FOLDER_NAME = "AppStorage";
        public readonly string CLIENT_APP_STORAGE_FOLDER_NAME = "ClientAppStorage";
        public AppDataStorage AppStorage { get; private set; } = null;
        private AppDataStorage _clientAppStorage = null;

        public AppServiceDataStorage(string folderName)
        {
            APPDATA_FOLDER = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);
            InitAppDataFolder();
        }

        private void InitAppDataFolder()
        {
            if(!Directory.Exists(APPDATA_FOLDER))
            {
                Directory.CreateDirectory(APPDATA_FOLDER);
            }
            InitAppsStorage();
        }

        private void InitAppsStorage()
        {
            AppStorage = GetStorage(APPSTORAGE_FOLDER_NAME);
        }

        public AppDataStorage GetAppStorage(AppInfo appInfo)
        {
            return GetStorage(appInfo.ID.ToString());
        }

        public AppDataStorage GetClientAppStorage()
        {
            if(_clientAppStorage == null)
            {
                _clientAppStorage = GetStorage(CLIENT_APP_STORAGE_FOLDER_NAME);
            }
            return _clientAppStorage;
        }

        private AppDataStorage GetStorage(string path)
        {
            string appStorage = Path.Combine(APPDATA_FOLDER, path);
            if (!Directory.Exists(appStorage))
            {
                Directory.CreateDirectory(appStorage);
            }
            return new AppDataStorage(appStorage);
        }
    }
}
