using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace VoIPLib
{
    class LowLevelMono2Stereo : ILowLevelVoiceEffect
    {

        public ISampleProvider GetSampleProvider(ISampleProvider input)
        {
            return new SampleProvider(input);
        }

        private class SampleProvider : ISampleProvider
        {

            private ISampleProvider output;

            public SampleProvider(ISampleProvider input)
            {
                if (input.WaveFormat.Channels == 2)
                    this.output = input;
                else
                    this.output = input.ToStereo();
            }

            public WaveFormat WaveFormat => output.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                return output.Read(buffer, offset, count);
            }
        }
    }
}
