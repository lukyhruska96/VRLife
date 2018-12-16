using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoIPLib
{
    public class MicrophoneStream
    {
        private readonly int bufferMilliseconds = 10;

        public int DeviceId { get; } = 0;

        public int Channels { get; } = 1;

        public int SampleRate { get;} = 44100;

        public int BitsPerSample { get; } = 16;

        private BufferedWaveProvider waveProvider;

        private WaveInEvent waveSource = new WaveInEvent();

        public MicrophoneStream()
        {
            this.waveSource.BufferMilliseconds = bufferMilliseconds;
            this.waveSource.WaveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
            this.waveProvider = new BufferedWaveProvider(this.waveSource.WaveFormat);
            this.waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(WaveSource_DataAvailable);
            this.waveSource.StartRecording();
        }

        public MicrophoneStream(int deviceId)
        {
            this.DeviceId = deviceId;
            this.waveSource.DeviceNumber = deviceId;
            this.waveSource.BufferMilliseconds = bufferMilliseconds;
            this.waveSource.WaveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
            this.waveProvider = new BufferedWaveProvider(this.waveSource.WaveFormat);
            this.waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(WaveSource_DataAvailable);
            this.waveSource.StartRecording();
        }

        public MicrophoneStream(int deviceId, int sampleRate, int channels)
        {
            this.DeviceId = deviceId;
            this.waveSource.DeviceNumber = deviceId;
            this.waveSource.BufferMilliseconds = bufferMilliseconds;
            this.SampleRate = sampleRate;
            this.Channels = channels;
            this.waveSource.WaveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
            this.waveProvider = new BufferedWaveProvider(this.waveSource.WaveFormat);
            this.waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(WaveSource_DataAvailable);
            this.waveSource.StartRecording();
        }

        public MicrophoneStream(int deviceId, int sampleRate, int bitsPerSample, int channels)
        {
            this.DeviceId = deviceId;
            this.waveSource.DeviceNumber = deviceId;
            this.waveSource.BufferMilliseconds = bufferMilliseconds;
            this.SampleRate = sampleRate;
            this.BitsPerSample = bitsPerSample;
            this.Channels = channels;
            this.waveSource.WaveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
            this.waveProvider = new BufferedWaveProvider(this.waveSource.WaveFormat);
            this.waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(WaveSource_DataAvailable);
            this.waveSource.StartRecording();
        }

        public IWaveProvider GetWaveProvider()
        {
            waveProvider.ClearBuffer();
            return waveProvider;
        }

        private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if(waveProvider.BufferedDuration > TimeSpan.FromSeconds(0.5))
            {
                waveProvider.ClearBuffer();
            }
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

    }
}
