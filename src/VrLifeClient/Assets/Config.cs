using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Logging;

namespace VrLifeClient
{
    class Config
    {
        public ILogger Loggers { get => _loggers; }
        private ILogger _loggers;
    }
}
