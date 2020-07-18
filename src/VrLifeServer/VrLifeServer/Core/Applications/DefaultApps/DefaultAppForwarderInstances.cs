using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Applications.DefaultApps.AppManager.Forwarder;
using VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppForwarderInstances : IEnumerable
    {
        public AppManagerForwarder AppManager { get; private set; } = new AppManagerForwarder();

        public VoiceChatAppForwarder VoiceChat { get; private set; } = new VoiceChatAppForwarder();

        public IEnumerator GetEnumerator()
        {
            yield return AppManager;
            yield return VoiceChat;
        }
    }
}
