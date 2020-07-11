using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    interface IFriendsManagementView
    {
        MenuItemGrid GetRoot();
        void Refresh();
    }
}
