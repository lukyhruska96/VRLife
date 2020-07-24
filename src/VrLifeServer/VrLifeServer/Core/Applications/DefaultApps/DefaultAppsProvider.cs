using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Provider.Core.Applications.DefaultApps;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.FriendsApp;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Applications.DefaultApps.ChatApp.Provider;
using VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppsProvider : IDefaultAppsProvider
    {
        public IFriendsAppProvider Friends { get; private set; } = new FriendsAppProvider();
        public IChatAppProvider Chat { get; private set; } = new ChatAppProvider();

        public IEnumerator GetEnumerator()
        {
            yield return Friends;
            yield return Chat;
        }
    }
}
