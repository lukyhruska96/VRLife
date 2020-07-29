using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Forwarder.API
{
    /// <summary>
    /// Interface ClosedAPI v případě Forwarder serveru
    /// </summary>
    public interface IClosedAPI
    {
        /// <summary>
        /// Instance OpenAPI
        /// </summary>
        IOpenAPI OpenAPI { get; }

        /// <summary>
        /// Instance poskytovatele služeb.
        /// </summary>
        IServiceProvider Services { get; }
    }
}
