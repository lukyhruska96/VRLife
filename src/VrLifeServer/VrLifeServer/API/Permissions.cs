using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeServer.Core.Applications.DefaultApps.AppManager.Provider;
using VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider;
using VrLifeShared.Core.Applications;

namespace VrLifeServer.API
{
    static class Permissions
    {
        private static ulong[] _defaultApps = new ulong[] { AppManagerProvider.APP_ID, FriendsAppProvider.APP_ID };
        private static Dictionary<ulong, bool> _allowedApps = new Dictionary<ulong, bool>();

        static Permissions()
        {
            _defaultApps.ToList().ForEach(x => _allowedApps[x] = true);
        }

        public static bool IsAllowed(AppInfo app)
        {
            return _allowedApps.ContainsKey(app.ID);
        }
    }
}
