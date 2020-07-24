using System;
using VrLifeAPI.Client.API.ClosedAPI.DeviceAPI.MicrophoneDevice;

namespace VrLifeAPI.Client.API.ClosedAPI
{
    public interface IDeviceAPI : IDisposable
    {
        IMicrophoneDevice Microphone { get; }
    }
}
