using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeShared.Core.Services.AppService
{
    public class AppPackageInfoException : Exception
    {
        public AppPackageInfoException(string message) : base(message)
        {
        }
    }
}
