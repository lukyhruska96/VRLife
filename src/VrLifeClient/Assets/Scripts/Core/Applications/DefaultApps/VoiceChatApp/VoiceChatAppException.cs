using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.VoiceChatApp
{
    class VoiceChatAppException : Exception
    {
        public VoiceChatAppException(string message) : base(message)
        {
        }
    }
}
