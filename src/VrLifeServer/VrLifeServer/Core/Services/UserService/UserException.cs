using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.UserService
{
    class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }
    }
}
