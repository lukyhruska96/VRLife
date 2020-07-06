using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services.TickRateService
{
    class TickRateServiceException : Exception
    {
        public TickRateServiceException(string message) : base(message)
        {
        }
    }
}
