using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services.EventService
{
    class EventServiceException : Exception
    {
        public EventServiceException(string message) : base(message)
        {
        }
    }
}
