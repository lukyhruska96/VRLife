using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeAPI;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppPackageInfo : IAppPackageInfo
    {
        public ulong ID { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public AppType Type { get; private set; }
        public AppVersion Version { get; private set; }
        public IAppPackageItemInfo Provider { get; private set; } = null;
        public IAppPackageItemInfo Forwarder { get; private set; } = null;
        public IAppPackageItemInfo Client { get; private set; } = null;

        public IAppPackageInfo[] Dependencies { get; private set; } = null;

        public AppPackageInfo(JObject obj)
        {
            ID = LoadJsonValue<ulong>(obj, "id");
            Name = LoadJsonValue<string>(obj, "name");
            Desc = LoadJsonValue<string>(obj, "desc");
            string type = LoadJsonValue<string>(obj, "type");
            switch(type.ToLower())
            {
                case "background":
                    Type = AppType.APP_BACKGROUND;
                    break;
                case "global":
                    Type = AppType.APP_GLOBAL;
                    break;
                case "menu":
                    Type = AppType.APP_MENU;
                    break;
                case "object":
                    Type = AppType.APP_OBJECT;
                    break;
                default:
                    throw new AppPackageInfoException("Invalid 'type' in app.json file.");
            }
            JArray dependencies = LoadJsonValue<JArray>(obj, "dependencies");
            List<AppPackageInfo> tmpDependencies = new List<AppPackageInfo>();
            foreach(JObject dependency in dependencies)
            {
                ulong id = LoadJsonValue<ulong>(dependency, "id");
                string strVer = LoadJsonValue<string>(dependency, "minVersion");
                tmpDependencies.Add(new AppPackageInfo(id, strVer));
            }
            Dependencies = tmpDependencies.ToArray();
            string strVersion = LoadJsonValue<string>(obj, "version");
            ParseVersion(strVersion);
            
            if(obj.ContainsKey("provider"))
            {
                Provider = new AppPackageItemInfo((JObject)obj["provider"]);
            }
            if(obj.ContainsKey("forwarder"))
            {
                Forwarder = new AppPackageItemInfo((JObject)obj["forwarder"]);
            }
            if(obj.ContainsKey("client"))
            {
                Client = new AppPackageItemInfo((JObject)obj["client"]);
            }
        }

        public AppPackageInfo(AppPackageInfoMsg msg)
        {
            ID = msg.AppId;
            Name = msg.Name;
            Desc = msg.Desc;
            Version = new AppVersion(msg.Version.ToArray());
            if(msg.ClientDll || msg.ClientZip)
            {
                Client = new AppPackageItemInfo(msg.ClientDll, msg.ClientZip);
            }
            if(msg.ForwarderDll || msg.ForwarderZip)
            {
                Forwarder = new AppPackageItemInfo(msg.ForwarderDll, msg.ForwarderZip);
            }
            Dependencies = msg.Dependencies
                ?.Select(x => new AppPackageInfo(x.AppId, new AppVersion(x.MinVersion.ToArray())))
                .ToArray();
        }

        public AppPackageInfo(AppPackageListEl msg)
        {
            ID = msg.AppId;
            Name = msg.Name;
            Desc = msg.Desc;
        }

        public AppPackageInfo(ulong id, string strVersion)
        {
            ID = id;
            ParseVersion(strVersion);
        }

        public AppPackageInfo(ulong id, AppVersion version)
        {
            ID = id;
            Version = version;
        }

        private void ParseVersion(string strVersion)
        {
            int[] ver = new int[3] { 0, 0, 0 };
            string[] version = strVersion.Split('.');
            for (int i = 0; i < version.Length && i < ver.Length; ++i)
            {
                if (!int.TryParse(version[i], out int val))
                {
                    throw new AppPackageInfoException("Invalid version format in app.json file.");
                }
                ver[i] = val;
            }
            Version = new AppVersion(ver);
        }

        public static T LoadJsonValue<T>(JObject obj, string key)
        {
            if (!obj.ContainsKey(key))
            {
                throw new AppPackageInfoException($"'{key}' key could not be found in app.json file.");
            }
            return obj[key].Value<T>();
        }

        public AppInfo ToAppInfo()
        {
            AppDependency[] deps = Dependencies?.Select(x => new AppDependency(x.ID, x.Version)).ToArray();
            return new AppInfo(ID, Name, Desc, Version, Type, deps);
        }

        public AppPackageInfoMsg ToNetworkModel()
        {
            AppPackageInfoMsg msg = new AppPackageInfoMsg();
            msg.AppId = ID;
            msg.Name = Name;
            msg.Desc = Desc;
            msg.Version.AddRange(Version.ToNumeric());
            msg.Dependencies.AddRange(Dependencies.Select(x => x.ToNetworkDependency()));
            msg.ForwarderDll = Forwarder != null;
            msg.ForwarderZip = Forwarder != null && Forwarder.ZipPath != null;
            msg.ClientDll = Client != null;
            msg.ClientZip = Client != null && Client.ZipPath != null;
            return msg;
        }

        public AppPackageDependency ToNetworkDependency()
        {
            AppPackageDependency msg = new AppPackageDependency();
            msg.AppId = ID;
            msg.MinVersion.AddRange(Version.ToNumeric());
            return msg;
        }

        public AppPackageListEl ToNetworkListEl()
        {
            AppPackageListEl el = new AppPackageListEl();
            el.AppId = ID;
            el.Name = Name;
            el.Desc = Desc;
            return el;
        }
    }

    public class AppPackageItemInfo : IAppPackageItemInfo
    {
        public string DLLPath { get; private set; } = null;
        public string ZipPath { get; private set; } = null;
        public AppPackageItemInfo(string dllPath, string zipPath = null)
        {
            DLLPath = dllPath;
            ZipPath = zipPath;
        }

        public AppPackageItemInfo(JObject obj)
        {
            DLLPath = AppPackageInfo.LoadJsonValue<string>(obj, "dllPath");
            if (obj.ContainsKey("zipPath"))
            {
                ZipPath = AppPackageInfo.LoadJsonValue<string>(obj, "zipPath");
            }
        }

        public AppPackageItemInfo(bool dll, bool zip)
        {
            if(dll)
            {
                DLLPath = "";
            }
            if(zip)
            {
                ZipPath = "";
            }
        }
    }
}
