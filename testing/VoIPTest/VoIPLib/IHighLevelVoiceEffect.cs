using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIPLib
{
    struct VoiceStreamStruct
    {
        public float ChannelBalance { get; set; }
        public int Volume { get; set; }
        public bool UnderNoiceLevel { get; set; }

        public byte[] Proccess(byte[] data, MicrophoneStream stream)
        {
            return null;
        }
    }

    interface IHighLevelVoiceEffect
    {
        Task<VoiceStreamStruct> ProccessAsync(Task<VoiceStreamStruct> data);
    }
}
