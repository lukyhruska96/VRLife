using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    public interface IMenuItemImage : IMenuItem, IGOReadable
    {

        void SetImage(Sprite img);

        void SetGif(Sprite[] frames, int fps);
    }
}
