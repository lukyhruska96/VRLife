using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.DeviceAPI.MicrophoneDevice
{
    class MicrophoneDeviceException : Exception
    {
        public MicrophoneDeviceException(string message) : base(message)
        {
        }
    }
}
