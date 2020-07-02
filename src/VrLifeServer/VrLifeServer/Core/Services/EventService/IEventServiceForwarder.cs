using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.EventService
{
    interface IEventServiceForwarder : IEventService, IServiceForwarder
    {
    }
}
