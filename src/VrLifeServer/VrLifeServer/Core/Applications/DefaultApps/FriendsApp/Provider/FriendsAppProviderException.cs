using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider
{
    class FriendsAppProviderException : Exception
    {
        public FriendsAppProviderException(string message) : base(message)
        {
        }
    }
}
