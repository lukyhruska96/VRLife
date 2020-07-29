using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu textu
    /// </summary>
    public interface IMenuItemText : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Setter zobrazeného textu.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        void SetText(string text);

        /// <summary>
        /// Setter rozsahu velikostí textu.
        /// </summary>
        /// <param name="sizeMin">Minimální povolená velikost textu při škálování.</param>
        /// <param name="sizeMax">Maximální povolená velikost textu při škálování.</param>
        void SetFontSize(int sizeMin, int sizeMax);

        /// <summary>
        /// Nastavení zarovnání textu.
        /// </summary>
        /// <param name="anchor">Struktura definující zarovnání textu.</param>
        void SetAlignment(TextAnchor anchor);

        /// <summary>
        /// Setter barvy textu.
        /// </summary>
        /// <param name="color">Barva textu.</param>
        void SetTextColor(Color color);

        /// <summary>
        /// Setter speciálních stylů textu.
        /// </summary>
        /// <param name="style">Styl textu.</param>
        void SetTextStyle(FontStyle style);
    }
}
