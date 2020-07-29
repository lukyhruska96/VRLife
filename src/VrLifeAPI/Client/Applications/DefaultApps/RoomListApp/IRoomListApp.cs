using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.Applications.DefaultApps.RoomListApp
{
    /// <summary>
    /// Interface pro výchozí aplikaci RoomListApp
    /// </summary>
    public interface IRoomListApp : IMenuApp
    {
        /// <summary>
        /// Dotaz pro změnu místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        void ChangeRoom(uint roomId);
    }
}
