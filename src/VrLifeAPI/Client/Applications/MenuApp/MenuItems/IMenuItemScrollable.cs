using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemScrollable : IMenuItem, IGOReadable
    {

        event Action Enabled;

        event Action Disabled;

        event Action<float> ScrollValueChanged;

        void Clear();

        void AddChildTop(IMenuItem child, float height);

        void AddChildBottom(IMenuItem child, float height);

        void AddChild(IMenuItem child, float height, int idx);
    }
}
