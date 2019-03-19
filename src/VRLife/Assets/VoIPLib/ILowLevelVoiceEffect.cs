using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIPLib
{
    public interface ILowLevelVoiceEffect
    {
        ISampleProvider GetSampleProvider(ISampleProvider input);
    }
}
