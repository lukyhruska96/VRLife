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

                // resize the buffer 
                // input is mono and output is stereo (need just a half of data from input)
                if (count / 2 > buff.Length)
                    buff = new float[count / 2];
                
                // get output from HRTF for current location info
                currOutput = hrtf.Get(location.Elev, location.Azim, location.Atten);

                // get data from input
                int read = input.Read(buff, 0, count / 2);

                // split data to size of filter buffer
                for (int i = 0; i < read / HRTF.FILTER_LEN; ++i)
                {
                    // convert raw real input to complex number
                    for (int j = 0; j < HRTF.FILTER_LEN; ++j)
                        currInput[j] = new Complex(buff[i * HRTF.FILTER_LEN + j], 0);
                    Process();
                    // save the output of Process method
                    Buffer.BlockCopy(dataOutput, 0, buffer, offset + i * HRTF.BUF_LEN * sizeof(float), HRTF.BUF_LEN * sizeof(float));
                }
                // if read returned smaller buffer not divisible by buffer size
                if (read % HRTF.FILTER_LEN != 0)
                {
                    int j;
                    for (j = 0; j < read % HRTF.FILTER_LEN; ++j)
                        currInput[j] = new Complex(buff[HRTF.FILTER_LEN * (read / HRTF.FILTER_LEN) + j], 0);
                    // fill the rest with noughts
                    for (j = read % HRTF.FILTER_LEN; j < HRTF.FILTER_LEN; ++j)
                        currInput[j] = Complex.Zero;
                    Process();
                    Buffer.BlockCopy(dataOutput, 0, buffer, offset + HRTF.BUF_LEN * (read / HRTF.FILTER_LEN) * sizeof(float), (read % HRTF.FILTER_LEN) * 2 * sizeof(float));
                }

                return read*2;
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
                    output[i] = (float) inverseBufferL[i / 2].Re;
                    output[i + 1] = (float) inverseBufferR[i / 2].Re;
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
