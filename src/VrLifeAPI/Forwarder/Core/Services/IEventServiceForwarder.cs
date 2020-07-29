using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.EventService;

namespace VrLifeAPI.Forwarder.Core.Services.EventService
{

    /// <summary>
    /// Interface služby událostí na straně Forwardera
    /// </summary>
    public interface IEventServiceForwarder : IEventService, IServiceForwarder
    {
    }
}
