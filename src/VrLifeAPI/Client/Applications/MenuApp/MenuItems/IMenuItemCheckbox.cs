using System;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu zaškrtávacího boxu
    /// </summary>
    public interface IMenuItemCheckbox : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Event volaný v případě změny hodnoty zaškrtávacího pole.
        /// </summary>
        event Action<bool> ValueChanged;

        /// <summary>
        /// Getter stavu zaškrtávacího pole.
        /// </summary>
        /// <returns>Aktuální hodnota.</returns>
        bool IsChecked();

        /// <summary>
        /// Setter stavu zaškrtávacího pole.
        /// </summary>
        /// <param name="val">Nastavovaná hodnota.</param>
        void SetValue(bool val);

        /// <summary>
        /// Setter textu popisujícího dané zaškrtávací pole.
        /// </summary>
        /// <param name="text">Text popisu.</param>
        void SetText(string text);
    }
}
