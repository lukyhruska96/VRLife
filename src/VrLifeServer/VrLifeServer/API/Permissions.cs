using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeAPI;
using VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider;
using VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder;
using VrLifeShared.Core.Applications;

namespace VrLifeServer.API
{
    static class Permissions
    {
        private static ulong[] _defaultApps = new ulong[] { FriendsAppProvider.APP_ID,
                                                            VoiceChatAppForwarder.APP_ID };
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
