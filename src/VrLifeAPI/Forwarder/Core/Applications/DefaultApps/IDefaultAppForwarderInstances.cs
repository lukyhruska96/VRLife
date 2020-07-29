using System;
using System.Collections;
using VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp;

namespace VrLifeAPI.Forwarder.Core.Applications.DefaultApps
{

    /// <summary>
    /// Interface poskytovatele výchozích aplikací pro danou instanci místnosti
    /// </summary>
    public interface IDefaultAppForwarderInstances : IEnumerable
    {
        /// <summary>
        /// Instance VoiceChat aplikace na straně Forwardera
        /// </summary>
        IVoiceChatAppForwarder VoiceChat { get; }
    }
}
