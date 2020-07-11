using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Applications.DefaultApps.AppManager.Provider;
using VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppsProvider : IEnumerable
    {
        public AppManagerProvider AppManager { get; private set; } = new AppManagerProvider();
        public FriendsAppProvider Friends { get; private set; } = new FriendsAppProvider();

        public IEnumerator GetEnumerator()
        {
            yield return AppManager;
            yield return Friends;
        }
    }
}
