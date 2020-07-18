using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API.GlobalAPI
{
    class PlayersAPIException : Exception
    {
        public PlayersAPIException(string message) : base(message)
        {
        }
    }
}
