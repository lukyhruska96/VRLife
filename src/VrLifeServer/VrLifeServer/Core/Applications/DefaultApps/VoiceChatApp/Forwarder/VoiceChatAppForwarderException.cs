using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder
{
    class VoiceChatAppForwarderException : Exception
    {
        public VoiceChatAppForwarderException(string message) : base(message)
        {
        }
    }
}
