using AForge.Math;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dSoundSynthesis
{
    public class HRTF
    {
        private string folderPath;

        public static readonly int FILTER_LEN = 512;

        public static readonly int BUF_LEN = FILTER_LEN * 2;

        private static readonly int MIN_AZIM = 0;

        private static readonly int MAX_AZIM = 345;

        private static readonly int STEP_AZIM = 15;

        private static readonly int N_AZIM = ((MAX_AZIM - MIN_AZIM) / STEP_AZIM)+1;

        private static readonly int N_ELEV = 6;
        
        public static readonly int MIN_ATTEN = 0; // in dB 

        public static readonly int MAX_ATTEN = 20;

        private Complex[][][][] hrtf = new Complex[N_AZIM][][][];

        // dictionary of indexes for elevation angles
        private static readonly Dictionary<int, int>  elevMap = new Dictionary<int, int>()
        {
            {0, 0 },
            {15, 1 },
            {30, 2 },
            {45, 3 },
            {315, 4 },
            {330, 5 },
            {345, 6 }
        };

        private static readonly int LEFT = 0;
        private static readonly int RIGHT = 1;

        public HRTF(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public void Init()
        {
            for(int i = MIN_AZIM; i <= MAX_AZIM; i += STEP_AZIM)
            {
                int azimIndex = i / STEP_AZIM;
                hrtf[azimIndex] = new Complex[N_ELEV][][];
                for(int j = 0; j < N_ELEV; ++j)
                {
                    hrtf[azimIndex][j] = new Complex[2][];
                    hrtf[azimIndex][j][LEFT] = new Complex[BUF_LEN / 2 + 1];
                    hrtf[azimIndex][j][RIGHT] = new Complex[BUF_LEN / 2 + 1];
                    float[] fileFloats;
                    int read;
                    using(WaveFileReader reader = new WaveFileReader(Path.Combine(folderPath, $"IRC_1004_C_R0195_T{i:000}_P{GetElev(j):000}.wav")))
                    {
                        ISampleProvider provider = reader.ToSampleProvider();
                        fileFloats = new float[reader.SampleCount * reader.WaveFormat.Channels];
                        read = provider.Read(fileFloats, 0, fileFloats.Length);
                    }
                    // conversion from HRIR to HRTF (FFT requires an array of length divisible my power of 2)
                    Complex[] tempLeft = new Complex[BUF_LEN / 2];
                    Complex[] tempRight = new Complex[BUF_LEN / 2];
                    for (int k = 0; k < fileFloats.Length/2; ++k)
                    {
                        tempLeft[k] = new Complex(fileFloats[k*2], 0);
                        tempRight[k] = new Complex(fileFloats[k*2+1], 0);
                    }
                    FourierTransform.FFT(tempLeft, FourierTransform.Direction.Forward);
                    FourierTransform.FFT(tempRight, FourierTransform.Direction.Forward);

                    // moving to local storage of HRTF values
                    for(int k = 0; k < BUF_LEN / 2; ++k)
                    {
                        hrtf[azimIndex][j][LEFT][k] = tempLeft[k];
                        hrtf[azimIndex][j][RIGHT][k] = tempRight[k];
                    }
                }
            }
        }

        private int GetElev(int elevIndex)
        {
            return elevMap.FirstOrDefault(x => x.Value == elevIndex).Key;
        }

        private int GetElevIndex(double elev)
        {
            if (elev > 45 && elev < 315)
                if (elev >= 135)
                    return elevMap[315];
                else
                    return elevMap[45];
            else if (elev > 345)
                if (elev >= 352.5)
                    return elevMap[0];
                else
                    return elevMap[90];
            else
                return elevMap[(int)Math.Round(elev / 15.0) * 15];
        }

        
        public HRTFOut Get(double elev, double azim, double atten)
        {
            bool flip = false;
            int elIndex = GetElevIndex(elev);
            azim %= 360;
            if (azim < 0)
                azim += 360;
            if(azim > 180)
            {
                azim -= 180;
                azim = 180 - azim; // TODO this sounds at least sometimes correctly (temp solution)
                flip = true;
            }
            int azimIndex = (int)Math.Round(azim / 15);
            double curr_atten = MIN_ATTEN + (MAX_ATTEN - MIN_ATTEN) * atten;
            double gain = Math.Pow(10.0, -curr_atten / 20);
            if (flip)
                return new HRTFOut { filtL = hrtf[azimIndex][elIndex][RIGHT], filtR = hrtf[azimIndex][elIndex][LEFT], gain = gain };
            else
                return new HRTFOut { filtL = hrtf[azimIndex][elIndex][LEFT], filtR = hrtf[azimIndex][elIndex][RIGHT], gain = gain };
        }
    }

    public struct HRTFOut
    {
        public Complex[] filtL;
        public Complex[] filtR;
        public double gain;
    }
}
