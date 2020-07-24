using System;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemCheckbox : IMenuItem, IGOReadable
    {
        event Action<bool> ValueChanged;

        bool IsChecked();

        void SetValue(bool val);

        void SetText(string text);
    }
}
