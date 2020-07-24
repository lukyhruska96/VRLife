using System;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemButton : IMenuItem, IGOReadable
    {
        event Action Clicked;

        void SetText(string text);

        void SetBgColor(Color color);

        void SetTextColor(Color color);

        void SetEnabled(bool status);
    }
}
