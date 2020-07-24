using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VrLifeAPI.Client.Applications.MenuApp.MenuItems
{
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
    public class MenuItemInfo
    {
        public MenuItemType Type { get; set; }
        public string Name { get; set; }
        public IMenuItem Parent { get; set; }
    }
}
