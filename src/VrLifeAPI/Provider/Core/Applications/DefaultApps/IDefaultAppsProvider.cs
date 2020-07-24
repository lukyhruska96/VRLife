using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.FriendsApp;

namespace VrLifeAPI.Provider.Core.Applications.DefaultApps
{
    public interface IDefaultAppsProvider : IEnumerable
    {
        IFriendsAppProvider Friends { get; }
        IChatAppProvider Chat { get; }
    }
}
