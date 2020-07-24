using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace VrLifeAPI.Client.API.GlobalAPI
{
    public interface IGlobalAPI
    {
        IPlayersAPI Players { get; }
    }
}
