using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppLoaderException : Exception
    {
        public AppLoaderException(string message) : base(message)
        {
        }
    }
}
