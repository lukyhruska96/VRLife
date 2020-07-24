using System;

namespace VrLifeAPI.Client.API.ClosedAPI.DeviceAPI.MicrophoneDevice 
{
    public interface IMicrophoneDevice : IDisposable
    {

        int Frequency { get; }
        int SampleLength { get; }
        int SampleDurationMS { get; }

        event Action<ulong, float[]> MicrophoneData;

        void SetMute(bool state);

        string[] GetDevices();

        void SetMic(int idx);
    }
}
