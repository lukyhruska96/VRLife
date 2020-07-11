using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.RoomListApp
{
    class RoomListAppException : Exception
    {
        public RoomListAppException(string message) : base(message)
        {
        }
    }
}
