using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu obrázku
    /// </summary>
    public interface IMenuItemImage : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Setter obrázku k vykreslení.
        /// </summary>
        /// <param name="img">Obrázek ve formě Sprite objektu.</param>
        void SetImage(Sprite img);

        /// <summary>
        /// Setter animovaného obrázku k vykreslení.
        /// </summary>
        /// <param name="frames">Individuální snímky animace ve formě pole Sprite objektů.</param>
        /// <param name="fps">Rychlost vykreslování animace v počtu snímků za vteřinu.</param>
        void SetGif(Sprite[] frames, int fps);
    }
}
