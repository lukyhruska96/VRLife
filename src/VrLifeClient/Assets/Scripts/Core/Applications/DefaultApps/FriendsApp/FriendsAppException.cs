using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsApp
{
    class FriendsAppException : Exception
    {
        public FriendsAppException(string message) : base(message)
        {

        }
    }
}
