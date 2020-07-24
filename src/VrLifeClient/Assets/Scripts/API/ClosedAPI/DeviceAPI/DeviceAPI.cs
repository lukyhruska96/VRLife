using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.ClosedAPI;
using VrLifeAPI.Client.API.ClosedAPI.DeviceAPI.MicrophoneDevice;

namespace VrLifeClient.API.DeviceAPI
{
    class DeviceAPI : IDeviceAPI
    {
        private IMicrophoneDevice _microphone = new MicrophoneDevice.MicrophoneDevice();
        public IMicrophoneDevice Microphone { get => _microphone; }
        public void Dispose()
        {
            _microphone.Dispose();
        }
    }
}
