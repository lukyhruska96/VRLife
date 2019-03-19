using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using VoIPLib;

namespace _3dSoundSynthesis
{
    public class ClickingFilter : ILowLevelVoiceEffect
    {
        public ISampleProvider GetSampleProvider(ISampleProvider input)
        {
            return new SampleProvider(input);
        }

        private class SampleProvider : ISampleProvider
        {
            private readonly ISampleProvider input;

            private float[] buffer = new float[0];

            private ArraySegment<float> loaded;

            public SampleProvider(ISampleProvider input)
            {
                this.input = input;
                this.loaded = new ArraySegment<float>(buffer, 0, 0);
            }

            public WaveFormat WaveFormat => input.WaveFormat;

            public int Read(float[] buffer, int offset, int count)
            {
                int i;
                    for(i = 0; i < count; ++i)
                if(this.buffer.Length < count * 2)
                {
                    float[] tmp = new float[count * 2];
                    for(i = loaded.Offset; i < loaded.Offset + loaded.Count; ++i)
                        tmp[i] = this.buffer[i];
                    this.buffer = tmp;
                    loaded = new ArraySegment<float>(this.buffer, loaded.Offset, loaded.Count);
                }

                int read = input.Read(this.buffer, loaded.Offset+loaded.Count, count * 2 - loaded.Count);
                
                if(loaded.Count + read < count)
                {
                    Buffer.BlockCopy(this.buffer, loaded.Offset, buffer, offset, loaded.Count + read);
                    loaded = new ArraySegment<float>(null, 0, 0);
                    return loaded.Count + read;
                }

                int size = 10;
                for (i = loaded.Offset + loaded.Count - size / 2; i < loaded.Offset + loaded.Count + size / 2; ++i)
                {
                    if (i < 0 || i >= buffer.Length)
                        continue;
                    float approxVal = 0;
                    for (int j = i; j < size; ++j)
                        approxVal += this.buffer[j];
                    buffer[i] = approxVal / size;
                }


                Buffer.BlockCopy(this.buffer, loaded.Offset, buffer, offset, count);
                
                for(i = 0; i < count; ++i)
                {
                    this.buffer[i] = this.buffer[loaded.Offset + count + i];
                }
                for (i = count; i < buffer.Length; ++i)
                    this.buffer[i] = 0;

                int loadedCount = loaded.Count;

                if (loaded.Offset != 0 || loaded.Count != count)
                    loaded = new ArraySegment<float>(this.buffer, 0, count);

                return loadedCount + read - count;
            }
        }
    }
}
