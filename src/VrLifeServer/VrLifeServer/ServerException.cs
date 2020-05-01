using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer
{
    class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
        }

        public ServerException(string message, Exception e) : base(message)
        {
        }
    }
}
