using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIPLib
{
    interface ILowLevelVoiceEffect
    {
        byte[] Proccess(byte[] data);
    }
}
