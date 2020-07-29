using System;
using System.Collections.Generic;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu tabulky
    /// </summary>
    public interface IMenuItemGrid : IMenuItem, IGOReadable
    {
        /// <summary>
        /// Event volaný v případě kliknutí na pole v mřížce se souřadnicema mřížky
        /// a instance nacházející se uvnitř dané souřadnice.
        /// </summary>
        event Action<IMenuItem,int,int> ItemClicked;

        /// <summary>
        /// Event volaný v případě, že se objekt stal viditelný.
        /// Používané k získání stavu, kdy se stala tato aplikace opět aktivní.
        /// </summary>
        event Action Enabled;

        /// <summary>
        /// Event volaný v případě zavření menu, nebo přechodu do jiné menu aplikace.
        /// </summary>
        event Action Disabled;

        /// <summary>
        /// Smazání všech objektů v tabulce vracející všechny jejich instance.
        /// Objekty stále existují v kořenové hierarchii (s parent rovným null).
        /// </summary>
        /// <returns>Seznam objektů, které byli smazány z tabulky.</returns>
        List<IMenuItem> Clear();

        /// <summary>
        /// Vložení objektu do tabulky.
        /// </summary>
        /// <param name="x">Počáteční x souřadnice levého hodního bodu objektu.</param>
        /// <param name="y">Počáteční y souřadnice levého hodního bodu objektu.</param>
        /// <param name="colWidth">Šířka objektu v počtu pokrývajících buněk.</param>
        /// <param name="colHeight">Výška objektu v počtu pokrývajících buněk.</param>
        /// <param name="item">Instance objektu k vložení.</param>
        void AddChild(int x, int y, int colWidth, int colHeight, IMenuItem item);
    }
}
