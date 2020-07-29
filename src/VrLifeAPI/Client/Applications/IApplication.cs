using System;

namespace VrLifeAPI.Client.Applications
{
    /// <summary>
    /// Interface každého typu aplikace.
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Getter Informace o aplikaci.
        /// </summary>
        /// <returns>Instance AppInfo.</returns>
        AppInfo GetInfo();
    }
}
