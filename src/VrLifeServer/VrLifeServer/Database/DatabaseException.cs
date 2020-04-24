using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Database
{
    class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }
}
