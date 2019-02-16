using AForge.Math;

namespace _3dSoundSynthesis
{

    // perfectly explained here:
    // https://www.eetimes.com/document.asp?doc_id=1275412
    public class BinauralSynthesis
    {

        private Complex[] currInput;

        private float[] dataOutput;

        private Complex[] fwdBuff = new Complex[HRTF.BUF_LEN];

        private Complex[] inverseBufferL = new Complex[HRTF.BUF_LEN];

        private Complex[] inverseBufferR = new Complex[HRTF.BUF_LEN];

        private Complex[] overlapL = new Complex[HRTF.BUF_LEN - HRTF.FILTER_LEN];

        private Complex[] overlapR = new Complex[HRTF.BUF_LEN - HRTF.FILTER_LEN];

        public BinauralSynthesis(Complex[] currInput, float[] dataOutput)
        {
            this.currInput = currInput;

            this.dataOutput = dataOutput;

            for (int i = 0; i < HRTF.BUF_LEN - HRTF.FILTER_LEN; ++i)
            {
                overlapL[i] = Complex.Zero;
                overlapR[i] = Complex.Zero;
            }
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

        public void Process(HRTFOut currOutput)
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
