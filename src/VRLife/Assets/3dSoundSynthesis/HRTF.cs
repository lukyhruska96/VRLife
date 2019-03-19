using AForge.Math;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        private static readonly int N_ELEV = 10;
        
        public static readonly int MIN_ATTEN = 0; // in dB 

        public static readonly int MAX_ATTEN = 20;

        private Complex[][][][] hrtf = new Complex[N_AZIM][][][];

        // these files always have angles 315, 330 and 345...missing angles are sometimes 60, 75, 90
        private int[] elevCountPerAzim = new int[]
        {
            10, 7, 8, 7, 9, 7, 8, 7, 9, 7, 8, 7, 9, 7, 8, 7, 9, 7, 8, 7, 9, 7, 8, 7
        };

        // dictionary of indexes for elevation angles
        private static readonly Dictionary<int, int>  elevMap = new Dictionary<int, int>()
        {
            {0, 0 },
            {15, 1 },
            {30, 2 },
            {45, 3 },
            {60, 4 },
            {75, 5 },
            {90, 6 },
            {315, 7 },
            {330, 8 },
            {345, 9 }
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
                    hrtf[azimIndex][j][LEFT] = new Complex[BUF_LEN];
                    hrtf[azimIndex][j][RIGHT] = new Complex[BUF_LEN];
                    if (j >= elevCountPerAzim[azimIndex] - 3 && j < N_ELEV - 3)
                        continue;
                    float[] fileFloats;
                    int read;
                    using(WaveFileReader reader = new WaveFileReader(Path.Combine(folderPath, $"IRC_1004_C_R0195_T{i:000}_P{GetElev(j):000}.wav")))
                    {
                        ISampleProvider provider = reader.ToSampleProvider();
                        fileFloats = new float[reader.SampleCount * reader.WaveFormat.Channels];
                        read = provider.Read(fileFloats, 0, fileFloats.Length);
                    }
                    // conversion from HRIR to HRTF (FFT requires an array of length divisible my power of 2)
                    int k;
                    for (k = 0; k < fileFloats.Length/2; ++k)
                    {
                        hrtf[azimIndex][j][LEFT][k] = new Complex(fileFloats[k*2], 0);
                        hrtf[azimIndex][j][RIGHT][k] = new Complex(fileFloats[k*2+1], 0);
                    }
                    for(k = fileFloats.Length; k < BUF_LEN; ++k)
                    {
                        hrtf[azimIndex][j][LEFT][k] = new Complex(0, 0);
                        hrtf[azimIndex][j][RIGHT][k] = new Complex(0, 0);
                    }
                    FourierTransform.FFT(hrtf[azimIndex][j][LEFT], FourierTransform.Direction.Forward);
                    FourierTransform.FFT(hrtf[azimIndex][j][RIGHT], FourierTransform.Direction.Forward);
                    float[] output = hrtf[azimIndex][j][LEFT].Select(x => (float)x.Re).ToArray();
                }
            }
        }

        private int GetElev(int elevIndex)
        {
            return elevMap.FirstOrDefault(x => x.Value == elevIndex).Key;
        }

        private int GetElevIndex(double elev, int azim)
        {
            int maxLower = (elevCountPerAzim[azim] - 3) * 15;
            if (elev > maxLower && elev < 315)
                if (elev >= (315 - maxLower) / 2)
                    return elevMap[315];
                else
                    return elevMap[maxLower];
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
            azim %= 360;
            if (azim < 0)
                azim += 360;
            if(azim > 180)
            {
                azim -= 180;
                azim = 180 - azim;
                flip = true;
            }
            int azimIndex = (int)Math.Round(azim / 15);
            int elIndex = GetElevIndex(elev, azimIndex);
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
