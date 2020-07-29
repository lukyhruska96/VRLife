using System;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu tlačítka.
    /// </summary>
    public interface IMenuItemButton : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Event volaný v případě, že bylo tlačítko stisknuto.
        /// </summary>
        event Action Clicked;

        /// <summary>
        /// Nastavení textu uvnitř tlačítka.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        void SetText(string text);

        /// <summary>
        /// Nastavení barvy pozadí tlačítka.
        /// </summary>
        /// <param name="color">Barva pozadí.</param>
        void SetBgColor(Color color);

        /// <summary>
        /// Nastavení barvy textu tlačítka.
        /// </summary>
        /// <param name="color">Barva textu.</param>
        void SetTextColor(Color color);

        /// <summary>
        /// Možnost vypnutí interakce s tlačítkem s efektem zašednutí.
        /// </summary>
        /// <param name="status">Nastavovaný stav tlačítka.</param>
        void SetEnabled(bool status);
    }
}
