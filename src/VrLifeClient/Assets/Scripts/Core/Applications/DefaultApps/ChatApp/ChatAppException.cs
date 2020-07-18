using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class ChatAppException : Exception
    {
        public ChatAppException(string message) : base(message)
        {
        }
    }
}
