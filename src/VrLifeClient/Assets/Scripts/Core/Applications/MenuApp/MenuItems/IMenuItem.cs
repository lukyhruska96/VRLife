using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    public interface IMenuItem : IDisposable
    {
        MenuItemInfo GetInfo();

        List<IMenuItem> GetChildren();

        void SetRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot);

        IMenuItem RemoveChild(IMenuItem child);

        void SetPadding(float left, float top, float right, float bottom);
        void SetPadding(float horizontal, float vertical);
    }

    // hiding this method from returned interface to the user
    public interface IGOReadable
    {
        GameObject GetGameObject();
    }


}
