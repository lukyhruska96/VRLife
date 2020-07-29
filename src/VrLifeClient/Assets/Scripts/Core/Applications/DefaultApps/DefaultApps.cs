using Assets.Scripts.Core.Applications.DefaultApps.AppManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.Applications.DefaultApps;
using VrLifeAPI.Client.Applications.DefaultApps.AppManager;
using VrLifeAPI.Client.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsManagementApp;
using VrLifeAPI.Client.Applications.DefaultApps.PlaceObjectApp;
using VrLifeAPI.Client.Applications.DefaultApps.RoomListApp;
using VrLifeAPI.Client.Applications.DefaultApps.VoiceChatApp;
using VrLifeClient.API;

namespace Assets.Scripts.Core.Applications.DefaultApps
{
    class DefaultApps : IDefaultApps
    {
        public IRoomListApp RoomList { get; private set; } = new RoomListApp.RoomListApp();
        public IAppManager AppManager { get; private set; } = new AppManager.AppManager();
        public IFriendsApp Friends { get; private set; } = new FriendsApp.FriendsApp();
        public IVoiceChatApp VoiceChat { get; private set; } = new VoiceChatApp.VoiceChatApp();
        public IFriendsManagementApp FriendsManagement { get; private set; } = 
            new FriendsManagementApp.FriendsManagementApp();
        public IChatApp Chat { get; private set; } = new ChatApp.ChatApp();
        public IPlaceObjectApp PlaceObject { get; private set; } = new PlaceObjectApp.PlaceObjectApp();

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return RoomList;
            yield return AppManager;
            yield return Friends;
            yield return FriendsManagement;
            yield return VoiceChat;
            yield return Chat;
            yield return PlaceObject;
        }
    }
}
