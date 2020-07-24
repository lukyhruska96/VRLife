using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.Applications.DefaultApps.RoomListApp
{
    public interface IRoomListApp : IMenuApp
    {
        void ChangeRoom(uint roomId);
    }
}
