using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.AppManager
{
    class AppManagerException : Exception
    {
        public AppManagerException(string message) : base(message)
        {
        }
    }
}
