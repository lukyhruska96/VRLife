using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp
{
    public interface IVoiceChatAppForwarder : IApplicationForwarder
    {

        void OnUserDisconnected(ulong userId, uint roomId, string reason);
    }
}
