using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemException : Exception
    {
        public MenuItemException(string message) : base(message)
        {
        }
    }
}
