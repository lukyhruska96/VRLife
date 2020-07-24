using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.TickRateService;

namespace VrLifeAPI.Forwarder.Core.Services.TickRateService
{
    public interface ITickRateServiceForwarder : ITickRateService, IServiceForwarder
    {
        bool IsActive(ulong userId, uint roomId);
        void SetSkeletonState(uint roomId, ulong userId, SkeletonState skeleton);
        void SetObjectState(uint roomId, ulong objectInstanceId, ObjectState obj);
    }
}
