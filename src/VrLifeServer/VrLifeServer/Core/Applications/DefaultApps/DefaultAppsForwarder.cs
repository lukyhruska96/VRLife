using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.Core.Applications.DefaultApps;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppsForwarder : IDefaultAppsForwarder
    {
        public Dictionary<uint, IDefaultAppForwarderInstances> DefaultApps { get; private set; } = 
            new Dictionary<uint, IDefaultAppForwarderInstances>();

        public IEnumerator GetEnumerator()
        {
            return DefaultApps.GetEnumerator();
        }
    }
}
