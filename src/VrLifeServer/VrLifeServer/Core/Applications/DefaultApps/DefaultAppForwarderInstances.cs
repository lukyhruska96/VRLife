using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Applications.DefaultApps.AppManager.Forwarder;

namespace VrLifeServer.Core.Applications.DefaultApps
{
    class DefaultAppForwarderInstances : IEnumerable
    {
        public AppManagerForwarder AppManager { get; private set; } = new AppManagerForwarder();

        public IEnumerator GetEnumerator()
        {
            yield return AppManager;
        }
    }
}
