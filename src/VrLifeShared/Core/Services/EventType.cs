using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeShared.Core.Services.EventService
{
    class EventType
    {
        public const uint MOVE_FORWARD = 1;
        public const uint MOVE_BACK = 2;
        public const uint MOVE_LEFT = 3;
        public const uint MOVE_RIGHT = 4;
        public const uint LOOK_MOVEMENT = 5;
        public const uint JUMP = 6;
        public const uint SELECT = 7;
        public const uint USE = 8;
    }
}
