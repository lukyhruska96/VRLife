using System;
using VrLifeAPI.Client.API.ClosedAPI.DeviceAPI.MicrophoneDevice;

namespace VrLifeAPI.Client.API.ClosedAPI
{
    /// <summary>
    /// struktura držící instance API pro lokální rozhraní.
    /// </summary>
    public interface IDeviceAPI : IDisposable
    {
        /// <summary>
        /// instance API pro záznamová zařízení
        /// </summary>
        IMicrophoneDevice Microphone { get; }
    }
}
