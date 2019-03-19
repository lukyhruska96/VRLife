using AForge.Math;
using NAudio.Wave;
using System;
using VoIPLib;

namespace _3dSoundSynthesis
{

    public class SourceLocation
    {
        public double Elev { get; set; }
        public double Azim { get; set; }
        public double Atten { get; set; }

        public SourceLocation(double elev, double azim, double atten)
        {
            this.Elev = elev;
            this.Azim = azim;
            this.Atten = atten;
        }
    }
    public class ICEFilter : ILowLevelVoiceEffect
    {
        private SourceLocation location;

        public ICEFilter(SourceLocation location)
        {
            this.location = location;
        }

        public ISampleProvider GetSampleProvider(ISampleProvider input)
        {
            return new SampleProvider(@"D:\OneDrive\Dokumenty\Škola\Ročníkový projekt\docs\HRTF", input, location);
        }

        private class SampleProvider : ISampleProvider
        {
            private float[] dataOutput = new float[HRTF.BUF_LEN];

            private Complex[] currInput = new Complex[HRTF.BUF_LEN];

            private ISampleProvider input;

            private SourceLocation location;

            private HRTF hrtf;

            private BinauralSynthesis binSyn;

            private HRTFOut currOutput;
            
            private int lastIdx = HRTF.BUF_LEN;

            private float[] buff = new float[4096];
            public WaveFormat WaveFormat { get; }

            public SampleProvider(string folderPath, ISampleProvider input, SourceLocation location)
            {
                this.hrtf = new HRTF(folderPath);
                this.hrtf.Init();
                this.binSyn = new BinauralSynthesis(currInput, dataOutput);
                this.location = location;

                // convert stereo input to mono
                if (input.WaveFormat.Channels != 1)
                    this.input = input.ToMono();
                else
                    this.input = input;

                WaveFormat = input.ToStereo().WaveFormat;
            }

            public int Read(float[] buffer, int offset, int count)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                int currIdx, filled, alignmentLength, totalRead, i, read, bufferOffset;
                currIdx = lastIdx;

                // size of not processed data from last call
                filled = HRTF.BUF_LEN - lastIdx;

                // resizing of buffer (up-rounded of variable count divisible by HRTF buffer size)
                alignmentLength = HRTF.FILTER_LEN - (((count - filled) / 2) % HRTF.FILTER_LEN);
                if (buff.Length < (count - filled) / 2 + alignmentLength)
                    buff = new float[(count - filled) / 2 + alignmentLength];

                // read data to buffer
                totalRead = input.Read(buff, 0, (count - filled) / 2 + alignmentLength);

                // copying of unprocessed part from last call
                for (i = 0; i < filled; ++i)
                    buffer[offset + i] = dataOutput[lastIdx + i];

                read = -1;
                bufferOffset = 0;

                currOutput = hrtf.Get(location.Elev, location.Azim, location.Atten);

                while (filled != count && read != 0)
                {
                    // read variable for this cycle
                    read = (filled - HRTF.BUF_LEN + currIdx) / 2 + HRTF.FILTER_LEN <= totalRead ? HRTF.FILTER_LEN : totalRead - (filled - HRTF.BUF_LEN + currIdx) / 2;

                    // reuse existing Complex data
                    for (i = 0; i < read; ++i)
                    {
                        currInput[i].Re = buff[bufferOffset + i];
                        currInput[i].Im = 0;
                    }
                    bufferOffset += read;
                    // fill missing by zero (in the end of transmission)
                    // read data not divisible by HRTF buffer size even that it should be
                    for (i = read; i < HRTF.FILTER_LEN; ++i)
                    {
                        currInput[i].Re = 0;
                        currInput[i].Im = 0;
                    }

                    // calling Binaural Synthesis
                    this.binSyn.Process(currOutput);

                    // saving first index of not returned part of this calculation
                    lastIdx = Math.Min(count - filled, read * 2);

                    Buffer.BlockCopy(dataOutput, 0, buffer, (offset + filled) * sizeof(float), lastIdx * sizeof(float));
                    filled += lastIdx;
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                return count;
            }
        }
    }
}
