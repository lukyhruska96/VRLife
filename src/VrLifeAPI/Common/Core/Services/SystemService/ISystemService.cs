using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.SystemService
{
    public interface ISystemService : IService
    {
        MainMessage CreateHelloMessage();
    }
}
