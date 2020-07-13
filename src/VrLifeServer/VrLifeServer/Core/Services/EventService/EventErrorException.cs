using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.EventService
{
    class EventErrorException : Exception
    {
        public EventErrorException(string message) : base(message)
        {
        }
    }
}
