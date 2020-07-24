using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemText : IMenuItem, IGOReadable
    {

        void SetText(string text);

        void SetFontSize(int sizeMin, int sizeMax);

        void SetAlignment(TextAnchor anchor);

        void SetTextColor(Color color);

        void SetTextStyle(FontStyle style);
    }
}
