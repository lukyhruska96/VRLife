using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp
{
    /// <summary>
    /// Interface VoiceChat aplikace na straně Forwardera
    /// </summary>
    public interface IVoiceChatAppForwarder : IApplicationForwarder
    {
        /// <summary>
        /// Metoda zavolaná v případě odpojení uživatele.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="reason">Důvod odpojení.</param>
        void OnUserDisconnected(ulong userId, uint roomId, string reason);
    }
}
