using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.EventService;

namespace VrLifeAPI.Forwarder.Core.Services.EventService
{
    public interface IEventServiceForwarder : IEventService, IServiceForwarder
    {
    }
}
