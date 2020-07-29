using System;
using UnityEngine.UI;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu textového vstupu.
    /// </summary>
    public interface IMenuItemInput : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Event volaný v případě, že se hodnota v textovém poli změnila
        /// s novou hodnotou v argumentu.
        /// </summary>
        event Action<string> ValueChanged;

        /// <summary>
        /// Event volaný v případě, že textové pole bylo odoznačeno
        /// s novou hodnotou v argumentu.
        /// </summary>
        event Action<string> EditEnded;

        /// <summary>
        /// Event volaný v případě stisknutí tlačítka enter,
        /// zatím co je objekt aktivní (kurzor uvnitř daného pole).
        /// </summary>
        event Action onSubmit;

        /// <summary>
        /// Getter aktuálního textu v textovém poli.
        /// </summary>
        /// <returns>Text v textovém poli.</returns>
        string GetText();

        /// <summary>
        /// Setter textu v textovém poli.
        /// </summary>
        /// <param name="text">Text k vložení.</param>
        void SetText(string text);

        /// <summary>
        /// Nastavení maximálního počtu znaků v daném textovém poli.
        /// </summary>
        /// <param name="limit">Max. počet znaků.</param>
        void SetCharLimit(int limit);

        /// <summary>
        /// Nastavení textu zobrazovaného zatím co element
        /// není aktivní. Tento text není součástí hodnoty v textovém poli.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        void SetPlaceholder(string text);

        /// <summary>
        /// Nastavení typu vkládaných typ.
        /// </summary>
        /// <param name="contentType">Typ dat.</param>
        void SetContentType(InputField.ContentType contentType);

        /// <summary>
        /// Nastavení víceřádkového vstupu.
        /// </summary>
        /// <param name="lineType">Typ řádkování.</param>
        void SetLineType(InputField.LineType lineType);
    }
}
