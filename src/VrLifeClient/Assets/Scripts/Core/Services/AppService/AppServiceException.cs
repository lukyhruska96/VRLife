using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services.AppService
{
    class AppServiceException : Exception
    {
        public AppServiceException(string message) : base(message)
        {
        }
    }
}
