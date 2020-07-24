using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Forwarder.Core.Applications.DefaultApps
{
    public interface IDefaultAppsForwarder : IEnumerable
    {
        Dictionary<uint, IDefaultAppForwarderInstances> DefaultApps { get; }
    }
}
