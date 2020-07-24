using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeClient;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    static class MenuItemUtils
    {
        private static IMenuAPI _menuAPI;
        static MenuItemUtils()
        {
            AppInfo info = new AppInfo(ulong.MaxValue, "MenuItemButton", null, new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_GLOBAL);
            _menuAPI = VrLifeCore.GetClosedAPI(info).MenuAPI;
        }

        public static void RunCoroutineSync(IEnumerator en, AutoResetEvent ev = null)
        {
            if(VrLifeCore.IsMainThread)
            {
                while (en.MoveNext()) ;
            }
            else
            {
                while(_menuAPI.StartCoroutine(en) == ulong.MaxValue) { Thread.Sleep(500); }
                ev?.WaitOne();
            }
        }
    }
}
