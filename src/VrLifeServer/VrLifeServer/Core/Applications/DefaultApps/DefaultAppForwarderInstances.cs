using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.Core.Applications.DefaultApps;
using VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp;
using VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppForwarderInstances : IDefaultAppForwarderInstances
    {

        public IVoiceChatAppForwarder VoiceChat { get; private set; } = new VoiceChatAppForwarder();

        public IEnumerator GetEnumerator()
        {
            yield return VoiceChat;
        }
    }
}
