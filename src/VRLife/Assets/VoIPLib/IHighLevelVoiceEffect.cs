using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIPLib
{
    public class VoiceStreamStruct
    {
        public float RightChannelBalance { get; set; } = 1.0f;
        public float LeftChannelBalance { get; set; } = 1.0f;
        public float Volume { get; set; } = 1.0f;
        public bool UnderNoiseLevel { get; set; }

        public ISampleProvider GetSampleProvider(ISampleProvider input)
        {
            return new SampleProvider(this, input);
        }

        private class SampleProvider : ISampleProvider
        {
            private VoiceStreamStruct voiceStreamStruct;

            private ISampleProvider input;

            private int buffSize = 4096;

            private float[] buffer;

            public SampleProvider(VoiceStreamStruct voiceStreamStruct, ISampleProvider input)
            {
                this.buffer = new float[buffSize];
                this.voiceStreamStruct = voiceStreamStruct;
                if (input.WaveFormat.Channels != 2)
                    this.input = input.ToStereo();
                else
                {
                    this.input = input;
                }
            }

            public WaveFormat WaveFormat => this.input.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                if (this.buffSize < count)
                {
                    this.buffer = new float[count];
                    this.buffSize = count;
                }
                int read = input.Read(this.buffer, 0, count);
                for (int i = 0; i < read; i+=2)
                {
                    if (voiceStreamStruct.UnderNoiseLevel)
                        buffer[i + offset] = 0;
                    else
                    {
                            buffer[i + offset] = voiceStreamStruct.LeftChannelBalance * voiceStreamStruct.Volume * this.buffer[i];
                            buffer[i + 1 + offset] = voiceStreamStruct.RightChannelBalance * voiceStreamStruct.Volume * this.buffer[i + 1];
                    }
                }
                return read;
            }
        }
    }

    public interface IHighLevelVoiceEffect
    {
        Task<VoiceStreamStruct> ProccessAsync(Task<VoiceStreamStruct> data);
    }
}
