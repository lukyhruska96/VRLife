using System;
using System.Collections;
using VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp;

namespace VrLifeAPI.Forwarder.Core.Applications.DefaultApps
{
    public interface IDefaultAppForwarderInstances : IEnumerable
    {

        IVoiceChatAppForwarder VoiceChat { get; }
    }
}
