using Assets.Scripts.Core.Applications;
using Assets.Scripts.Core.Applications.DefaultApps.AppManager;
using Assets.Scripts.Core.Applications.DefaultApps.RoomListApp;
using Assets.Scripts.Core.Applications.DefaultApps.VoiceChatApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.API
{
    static class Permissions
    {
        private const ulong INTERNAL_APP = ulong.MaxValue;
        private static Dictionary<ulong, bool> _allowedApps = new Dictionary<ulong, bool>();
        private static readonly ulong[] defaultApps = new ulong[]{ AppManager.APP_ID, RoomListApp.APP_ID, VoiceChatApp.APP_ID };
        static Permissions()
        {
            _allowedApps.Add(INTERNAL_APP, true);
            defaultApps.ToList().ForEach(x => _allowedApps.Add(x, true));
        }

        public static bool IsAllowed(AppInfo info)
        {
            return _allowedApps.ContainsKey(info.ID);
        }
    }
}
