using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Interface pro wrapper menu skrolovatelného pole
    /// </summary>
    public interface IMenuItemScrollable : IMenuItem, IGOReadable
    {
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
        /// Event volaný v případě změny hodnoty ve scrollbaru (hodnota <0;1>)
        /// Využitelné v případě donačítávání nových dat v případě dočtení do určité části
        /// zobrazeného textu.
        /// </summary>
        event Action<float> ScrollValueChanged;

        /// <summary>
        /// Smazání všech objektů uvnitř dané struktury (které měli parent rovný aktuálnímu objektu)
        /// </summary>
        void Clear();

        /// <summary>
        /// Vložení nového objektu nahoru jako první element.
        /// </summary>
        /// <param name="child">Objekt ke vložení.</param>
        /// <param name="height">Výška objektu v px.</param>
        void AddChildTop(IMenuItem child, float height);

        /// <summary>
        /// Vložení nového objektu dolů jako poslední element.
        /// </summary>
        /// <param name="child">Objekt ke vložení.</param>
        /// <param name="height">Výška objektu v px.</param>
        void AddChildBottom(IMenuItem child, float height);

        /// <summary>
        /// Vložení nového objektu na zvolený index.
        /// </summary>
        /// <param name="child">Objekt ke vložení.</param>
        /// <param name="height">Výška objektu v px.</param>
        /// <param name="idx">Index pozice k vložení.</param>
        void AddChild(IMenuItem child, float height, int idx);
    }
}
