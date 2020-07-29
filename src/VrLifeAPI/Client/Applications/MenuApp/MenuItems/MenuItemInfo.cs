using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
    /// <summary>
    /// Enum typu menu objektu
    /// </summary>
    public enum MenuItemType
    {
        MI_MAINFRAME,
        MI_GROUPBOX,
        MI_GRID,
        MI_BROWSER,
        MI_TEXT,
        MI_IMAGE,
        MI_INPUT,
        MI_BUTTON,
        MI_CHECKBOX,
        MI_SCROLLABLE
    }
    
    /// <summary>
    /// Informační objekt pro daný menu objekt
    /// </summary>
    public class MenuItemInfo
    {
        /// <summary>
        /// Typ objektu.
        /// </summary>
        public MenuItemType Type { get; set; }

        /// <summary>
        /// Název objektu v hierarchii GameObjectů
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rodič daného objektu.
        /// </summary>
        public IMenuItem Parent { get; set; }
    }
}
