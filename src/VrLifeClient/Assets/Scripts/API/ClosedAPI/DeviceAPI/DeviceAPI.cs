using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.DeviceAPI
{
    class DeviceAPI : IDisposable
    {
        private MicrophoneDevice.MicrophoneDevice _microphone = new MicrophoneDevice.MicrophoneDevice();
        public MicrophoneDevice.MicrophoneDevice Microphone { get => _microphone; }
        public void Dispose()
        {
            _microphone.Dispose();
        }
    }
}
