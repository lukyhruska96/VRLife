using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services
{
    /// <summary>
    /// Extension třída pro služby
    /// </summary>
    public static class ServiceUtils
    {
        /// <summary>
        /// Vytvoření chybové zprávy na událost.
        /// </summary>
        /// <param name="msgId">ID chybné zprávy.</param>
        /// <param name="errType">ID typu chyby.</param>
        /// <param name="errCode">ID chybové hlášky.</param>
        /// <param name="errMsg">Textová chybová hláška.</param>
        /// <returns>EventResponse objekt k odeslání</returns>
        public static EventResponse CreateErrorResponse(ulong msgId, uint errType, uint errCode, string errMsg = null)
        {
            EventResponse msg = new EventResponse();
            msg.Error = new ErrorMsg();
            msg.Error.MsgId = msgId;
            msg.Error.ErrorType = errType;
            msg.Error.ErrorCode = errCode;
            msg.Error.ErrorMsg_ = errMsg;
            return msg;
        }

        /// <summary>
        /// Vytvoření chybové zprávy.
        /// </summary>
        /// <param name="msgId">ID chybné zprávy.</param>
        /// <param name="errType">ID typu chyby.</param>
        /// <param name="errCode">ID chybové hlášky.</param>
        /// <param name="errMsg">Textová chybová hláška.</param>
        /// <returns>MainMessage objekt k odeslání.</returns>
        public static MainMessage CreateErrorMessage(ulong msgId, uint errType, uint errCode, string errMsg = null)
        {
            ErrorMsg errorMsg = new ErrorMsg();
            errorMsg.MsgId = msgId;
            errorMsg.ErrorType = errType;
            errorMsg.ErrorCode = errCode;
            errorMsg.ErrorMsg_ = errMsg;
            SystemMsg sysMsg = new SystemMsg();
            sysMsg.ErrorMsg = errorMsg;
            MainMessage msg = new MainMessage();
            msg.SystemMsg = sysMsg;
            return msg;

        }

        /// <summary>
        /// Vytvoření stavové zprávy OK.
        /// </summary>
        /// <param name="msgId">Id přijaté zprávy.</param>
        /// <returns>MainMessage objekt k odeslání.</returns>
        public static MainMessage CreateOkMessage(ulong msgId = 0)
        {
            OkMsg okMsg = new OkMsg();
            okMsg.MsgId = msgId;
            SystemMsg sysMsg = new SystemMsg();
            sysMsg.OkMsg = okMsg;
            MainMessage msg = new MainMessage();
            msg.SystemMsg = sysMsg;
            return msg;
        }

        /// <summary>
        /// Vytvoření zprávy přesměrování.
        /// </summary>
        /// <param name="recvMsg">Přijatá zpráva.</param>
        /// <param name="address">Adresa na kterou je zpráva přesměrována.</param>
        /// <param name="port">Port na který je zpráva přesměrována.</param>
        /// <returns>MainMessage objekt k odeslání.</returns>
        public static MainMessage CreateRedirectMessage(MainMessage recvMsg, int address, int port)
        {
            RedirectMsg redirectMsg = new RedirectMsg();
            redirectMsg.Address = address;
            redirectMsg.Port = port;
            redirectMsg.ReceivedMsg = ByteString.CopyFrom(recvMsg.ToByteArray());
            MainMessage msg = new MainMessage();
            msg.SystemMsg = new SystemMsg();
            msg.SystemMsg.RedirectMsg = redirectMsg;
            return msg;
        }

        /// <summary>
        /// Vytvoření zprávy přesměrování. 
        /// </summary>
        /// <param name="recvMsg">Přijatá zpráva.</param>
        /// <param name="ip">Adresa na kterou je zpráva přesměrována</param>
        /// <returns>MainMessage objekt k odeslání.</returns>
        public static MainMessage CreateRedirectMessage(MainMessage recvMsg, IPEndPoint ip)
        {
            return ServiceUtils.CreateRedirectMessage(recvMsg, ip.Address.ToInt(), ip.Port);
        }

        /// <summary>
        /// Kontrola zda je zpráva chybovou systémovou hláškou.
        /// </summary>
        /// <param name="msg">Zpráva ke kontrole.</param>
        /// <returns>true - chybová zpráva, false - jiná</returns>
        public static bool IsError(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg &&
                msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
        }
    }
}
