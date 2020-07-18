using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.API.HUDAPI
{
    public struct Notification
    {
        public string Header { get; private set; }
        public ulong AppId { get; private set; }
        public string Text { get; private set; }
        public string ActionPath { get; private set; }

        public Notification(AppInfo appInfo, string text, string actionPath = null)
        {
            Header = appInfo.Name;
            AppId = appInfo.ID;
            Text = text;
            ActionPath = actionPath;
        }
    }
}
