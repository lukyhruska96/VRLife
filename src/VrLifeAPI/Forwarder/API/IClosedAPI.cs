using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Forwarder.API
{
    public interface IClosedAPI
    {
        IOpenAPI OpenAPI { get; }
        IServiceProvider Services { get; }
    }
}
