using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.FriendsApp;

namespace VrLifeAPI.Provider.Core.Applications.DefaultApps
{
    /// <summary>
    /// Interface poskytovatele výchozích aplikací na straně Providera.
    /// </summary>
    public interface IDefaultAppsProvider : IEnumerable
    {
        /// <summary>
        /// Instance FriendsApp
        /// </summary>
        IFriendsAppProvider Friends { get; }

        /// <summary>
        /// Instance ChatApp
        /// </summary>
        IChatAppProvider Chat { get; }
    }
}
