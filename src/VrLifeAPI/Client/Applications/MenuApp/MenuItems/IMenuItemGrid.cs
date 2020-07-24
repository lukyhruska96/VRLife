using System;
using System.Collections.Generic;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemGrid : IMenuItem, IGOReadable
    {
        event Action<IMenuItem,int,int> ItemClicked;

        event Action Enabled;

        event Action Disabled;

        List<IMenuItem> Clear();

        void AddChild(int x, int y, int colWidth, int colHeight, IMenuItem item);
    }
}
