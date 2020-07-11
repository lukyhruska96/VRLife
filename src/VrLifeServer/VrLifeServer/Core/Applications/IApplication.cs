using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace VrLifeServer.Applications
{
    interface IApplication : IDisposable
    {
        AppInfo GetInfo();
    }
}
