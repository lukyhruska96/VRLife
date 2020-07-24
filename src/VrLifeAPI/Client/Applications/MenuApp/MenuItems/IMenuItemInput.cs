using System;
using UnityEngine.UI;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemInput : IMenuItem, IGOReadable
    {
        event Action<string> ValueChanged;

        event Action<string> EditEnded;

        event Action onSubmit;

        string GetText();

        void SetText(string text);

        void SetCharLimit(int limit);

        void SetPlaceholder(string text);

        void SetContentType(InputField.ContentType contentType);

        void SetLineType(InputField.LineType lineType);
    }
}
