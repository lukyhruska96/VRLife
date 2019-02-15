using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoIPLib;
using NAudio.Wave;
using AForge.Math;
using System.IO;

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

    // perfectly explained here:
    // https://www.eetimes.com/document.asp?doc_id=1275412
    public class BinauralSynthesis : ILowLevelVoiceEffect
    {
        SourceLocation location;
        public BinauralSynthesis(SourceLocation location)
        {
            this.location = location;
        }

        public ISampleProvider GetSampleProvider(ISampleProvider input)
        {
            return new SampleProvider(Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\..\..\..\..\docs\HRTF"), input, location);
        }

        private class SampleProvider : ISampleProvider
        {

            private SourceLocation location;

            private ISampleProvider input;

            private HRTF hrtf;

            private HRTFOut currOutput;

            private Complex[] currInput = new Complex[HRTF.BUF_LEN];

            private float[] dataOutput = new float[HRTF.BUF_LEN];

            private float[] buff = new float[4096];

            private Complex[] fwdBuff = new Complex[HRTF.BUF_LEN];

            private Complex[] inverseBufferL = new Complex[HRTF.BUF_LEN];

            private Complex[] inverseBufferR = new Complex[HRTF.BUF_LEN];

            private Complex[] overlapL = new Complex[HRTF.BUF_LEN - HRTF.FILTER_LEN];

            private Complex[] overlapR = new Complex[HRTF.BUF_LEN - HRTF.FILTER_LEN];

            private int lastIdx = 0;

            public SampleProvider(string folderPath, ISampleProvider input, SourceLocation location)
            {
                this.hrtf = new HRTF(folderPath);
                this.hrtf.Init();
                this.location = location;

                // convert stereo input to mono
                if (input.WaveFormat.Channels != 1)
                    this.input = input.ToMono();
                else
                    this.input = input;

                WaveFormat = input.ToStereo().WaveFormat;

                for(int i = 0; i < HRTF.BUF_LEN - HRTF.FILTER_LEN; ++i)
                {
                    overlapL[i] = Complex.Zero;
                    overlapR[i] = Complex.Zero;
                }
            }

            public WaveFormat WaveFormat { get; }

            public int Read(float[] buffer, int offset, int count)
            {
                if (buff.Length < Math.Ceiling((float)count / HRTF.BUF_LEN) * HRTF.FILTER_LEN)
                    buff = new float[(int)Math.Ceiling((float)count / HRTF.BUF_LEN) * HRTF.FILTER_LEN];
                currOutput = hrtf.Get(location.Elev, location.Azim, location.Atten);
                count -= HRTF.BUF_LEN - lastIdx;
                offset += HRTF.BUF_LEN - lastIdx;
                int i, j, buffOffset, moveSize;
                for (i = 0; i < HRTF.BUF_LEN - lastIdx; ++i)
                    buffer[i] = dataOutput[lastIdx + i];
                int read = -1;
                i = 0;
                int totalRead = input.Read(buff, 0, (int)Math.Ceiling((float)count / HRTF.BUF_LEN) * HRTF.FILTER_LEN);
                buffOffset = 0;
                while (i < count/2 && read != 0)
                {
                    read = i + HRTF.FILTER_LEN < totalRead ? HRTF.FILTER_LEN : totalRead % HRTF.FILTER_LEN;
                    for(j = 0; j < read; ++j)
                        currInput[j] = new Complex(buff[buffOffset + j], 0);
                    buffOffset += read;
                    for(j = read; j < HRTF.FILTER_LEN; ++j)
                        currInput[j] = new Complex(0, 0);
                    Process();
                    moveSize = HRTF.FILTER_LEN * 2 * sizeof(float);
                    if (i + HRTF.FILTER_LEN >= count / 2)
                    {
                        moveSize = (count - (i * 2)) * sizeof(float);
                        lastIdx = moveSize == 0 ? HRTF.BUF_LEN : moveSize / sizeof(float);
                    }
                    else
                        lastIdx = HRTF.BUF_LEN;
                    Buffer.BlockCopy(dataOutput, 0, buffer, offset * sizeof(float) + i * 2 * sizeof(float), moveSize);

                    i += read;
                }

                return count;
            }

            private void InverseTransformation(Complex[] filtL, Complex[] filtR, double gain, float[] output)
            {
                int i;
                // Spectral multiply transform
                OptimizedComplexMultiply(fwdBuff, filtL, inverseBufferL, HRTF.BUF_LEN);
                OptimizedComplexMultiply(fwdBuff, filtR, inverseBufferR, HRTF.BUF_LEN);

                // inverse FFT transform (backward FFT, so it needs to be scaled back)
                FourierTransform.FFT(inverseBufferL, FourierTransform.Direction.Backward);
                FourierTransform.FFT(inverseBufferR, FourierTransform.Direction.Backward);

                // scaling + applying of gain
                for (i = 0; i < HRTF.BUF_LEN; ++i)
                {
                    inverseBufferL[i] = inverseBufferL[i] / (gain / HRTF.BUF_LEN);
                    inverseBufferR[i] = inverseBufferR[i] / (gain / HRTF.BUF_LEN);
                }

                // overlap-add
                for (i = 0; i < HRTF.FILTER_LEN; ++i)
                {
                    inverseBufferL[i] += overlapL[i];
                    inverseBufferR[i] += overlapR[i];
                }

                // saving overlap for next call
                for (i = HRTF.FILTER_LEN; i < HRTF.BUF_LEN; ++i)
                {
                    overlapL[i - HRTF.FILTER_LEN] = inverseBufferL[i];
                    overlapR[i - HRTF.FILTER_LEN] = inverseBufferR[i];
                }

                // Interleave left and right channels into output buffer 
                for (i = 0; i < HRTF.BUF_LEN; i += 2)
                {
                    output[i] = (float)inverseBufferL[i / 2].Re;
                    output[i + 1] = (float)inverseBufferR[i / 2].Re;
                }
            }

            private void Process()
            {
                int i;

                // copy current data in buffer from its half
                for (i = 0; i < HRTF.FILTER_LEN; ++i)
                    fwdBuff[i] = currInput[i];
                for (i = HRTF.FILTER_LEN; i < HRTF.BUF_LEN; ++i)
                {
                    fwdBuff[i].Re = 0;
                    fwdBuff[i].Im = 0;
                }

                // convert time domain to frequency domain using forward FFT
                FourierTransform.FFT(fwdBuff, FourierTransform.Direction.Forward);

                // do the fast convolution
                InverseTransformation(currOutput.filtL, currOutput.filtR, currOutput.gain, dataOutput);
            }

            // Optimized multiply of two conjugate symmetric arrays (c = a * b).
            // Author: Bill Gardner
            private void OptimizedComplexMultiply(Complex[] a, Complex[] b, Complex[] c, long size)
            {
                long half = size >> 1;
                long i;
                for (i = 0; i <= half; ++i)
                    c[i] = a[i] * b[i];
                for (i = 0; i < half; i++)
                {
                    c[half + i].Re = c[half - i].Re;
                    c[half + i].Im = -c[half - i].Im;
                }
            }
        }
    }
}
