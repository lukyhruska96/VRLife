using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking
{
    public class ErrorMsgException : Exception
    {
        private ulong _msgId;
        public ulong MsgId { get => _msgId; }

        private uint _errorType;
        public uint ErrorType { get => _errorType; }

        private uint _errorCode;
        public uint ErrorCode { get => _errorCode; }

        private string _errorMsg;
        public string ErrorMsg { get => _errorMsg; }

        public ErrorMsgException(string message) : base(message)
        {
        }

        public ErrorMsgException(ErrorMsg msg) : base(msg.ErrorMsg_)
        {
            this._msgId = msg.MsgId;
            this._errorType = msg.ErrorType;
            this._errorCode = msg.ErrorCode;
            this._errorMsg = msg.ErrorMsg_;
        }
    }
}
