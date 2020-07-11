using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceException : Exception
    {
        public AppServiceException(string message) : base(message)
        {
        }
    }
}
