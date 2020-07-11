using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeShared.Core.Applications
{
    public enum AppType
    {
        APP_BACKGROUND,
        APP_MENU,
        APP_OBJECT,
        APP_GLOBAL
    }
    public class AppInfo
    {
        public AppInfo(ulong id, string name, string desc, AppType type, ulong[] requires = null)
        {
            this.ID = id;
            this.Name = name;
            this.Description = desc;
            this.Type = type;
            this.Requires = null;
        }

        public ulong ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AppType Type { get; private set; }
        public ulong[] Requires { get; private set; }

    }
}
