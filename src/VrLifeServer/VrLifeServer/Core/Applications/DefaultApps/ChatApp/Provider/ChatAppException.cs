using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Applications.DefaultApps.ChatApp.Provider
{
    class ChatAppException : Exception
    {
        public ChatAppException(string message) : base(message)
        {
        }
    }
}
