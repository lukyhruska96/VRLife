using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services.RoomService
{
    class RoomServiceException : Exception
    {
        public RoomServiceException(string message) : base(message)
        {
        }
    }
}
