using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace VrLifeServer.Core.Services.TickRateService
{
    struct TickState
    {
        public ulong Tick { get; set; }
        public ImmutableDictionary<ulong, SkeletonState> States { get; set; }

        public TickState(ulong tick)
        {
            Tick = tick;
        }
    }
}
