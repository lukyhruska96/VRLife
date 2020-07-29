using System;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Applications
{
    /// <summary>
    /// Enum odesílatele zprávy.
    /// </summary>
    public enum SenderType
    {
        USER, SERVER
    }

    /// <summary>
    /// Kontext zprávy pro zpracování zprávy pro aplikaci.
    /// </summary>
    public struct MsgContext
    {
        /// <summary>
        /// Unikátní ID zprávy.
        /// </summary>
        public ulong msgId;

        /// <summary>
        /// Odesílatel zprávy.
        /// </summary>
        public SenderType senderType;

        /// <summary>
        /// ID odesílatele.
        /// </summary>
        public ulong senderId;

        /// <summary>
        /// Konstruktor ze síťového objektu hlavní zprávy.
        /// </summary>
        /// <param name="msg">Hlavní přijatá zpráva.</param>
        public MsgContext(MainMessage msg)
        {
            msgId = msg.MsgId;
            senderType = msg.SenderIdCase == MainMessage.SenderIdOneofCase.ClientId ? SenderType.USER : SenderType.SERVER;
            senderId = senderType == SenderType.USER ? msg.ClientId : msg.ServerId;
        }
    }

    /// <summary>
    /// Interface serverové aplikace.
    /// </summary>
    public interface IApplicationServer : IApplication
    {
        /// <summary>
        /// Zpracování přijaté zprávy pro danou aplikací.
        /// </summary>
        /// <param name="data">Přijatá data ve formě byte array.</param>
        /// <param name="size">Délka pole.</param>
        /// <param name="ctx">Kontext zprávy.</param>
        /// <returns>Návratová data ve formě byte array.</returns>
        byte[] HandleMessage(byte[] data, int size, MsgContext ctx);

        /// <summary>
        /// Zpracování přijaté události pro danou aplikaci.
        /// 
        /// Odpověď je poté zabalena do síťovacího objektu EventResponse.
        /// </summary>
        /// <param name="eventData">Data události.</param>
        /// <param name="ctx">Kontext události.</param>
        /// <returns>Odpověď ve formě byte array.</returns>
        byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx);
    }
}
