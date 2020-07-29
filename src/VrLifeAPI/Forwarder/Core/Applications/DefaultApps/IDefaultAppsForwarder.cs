using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Forwarder.Core.Applications.DefaultApps
{
    /// <summary>
    /// Interface poskytovatele výchozích aplikací na straně Forwardera
    /// </summary>
    public interface IDefaultAppsForwarder : IEnumerable
    {
        /// <summary>
        /// Slovník instancí poskytovatelů výchozích aplikací pro danou instanci místnosti.
        /// Hashování podle ID místnosti.
        /// </summary>
        Dictionary<uint, IDefaultAppForwarderInstances> DefaultApps { get; }
    }
}
