using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppsForwarder : IEnumerable
    {
        public Dictionary<uint, DefaultAppForwarderInstances> DefaultApps { get; private set; } = 
            new Dictionary<uint, DefaultAppForwarderInstances>();

        public IEnumerator GetEnumerator()
        {
            return DefaultApps.GetEnumerator();
        }
    }
}
