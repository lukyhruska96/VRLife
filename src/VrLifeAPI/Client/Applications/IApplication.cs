using System;

namespace VrLifeAPI.Client.Applications
{
    public interface IApplication : IDisposable
    {
        AppInfo GetInfo();
    }
}
