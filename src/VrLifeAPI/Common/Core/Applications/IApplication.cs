using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace VrLifeAPI.Common.Core.Applications
{
    /// <summary>
    /// Interface serverové aplikace
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Getter informací o dané aplikaci.
        /// </summary>
        /// <returns>Instance AppInfo.</returns>
        AppInfo GetInfo();
    }
}
