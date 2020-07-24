using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.AppService
{
    public interface IAppPackageInfo
    {
        ulong ID { get; }
        string Name { get; }
        string Desc { get; }
        AppType Type { get; }
        AppVersion Version { get; }
        IAppPackageItemInfo Provider { get; }
        IAppPackageItemInfo Forwarder { get; }
        IAppPackageItemInfo Client { get; }
        IAppPackageInfo[] Dependencies { get; }

        AppInfo ToAppInfo();
        AppPackageInfoMsg ToNetworkModel();
        AppPackageListEl ToNetworkListEl();
        AppPackageDependency ToNetworkDependency();
    }

    public interface IAppPackageItemInfo
    {
        string DLLPath { get; }
        string ZipPath { get; }
    }
}
