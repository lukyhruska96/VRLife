using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeClient.Core.Services.UserService
{
    class UserException : Exception
    {
        public UserException(string message) : base(message)
        {
        }
    }
}
