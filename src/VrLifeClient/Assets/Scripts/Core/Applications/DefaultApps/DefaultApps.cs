using Assets.Scripts.Core.Applications.DefaultApps.AppManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps
{
    class DefaultApps : IEnumerable
    {
        public RoomListApp.RoomListApp RoomList { get; private set; } = new RoomListApp.RoomListApp();
        public AppManager.AppManager AppManager { get; private set; } = new AppManager.AppManager();
        public FriendsApp.FriendsApp Friends { get; private set; } = new FriendsApp.FriendsApp();
        public VoiceChatApp.VoiceChatApp VoiceChat { get; private set; } = new VoiceChatApp.VoiceChatApp();
        public FriendsManagementApp.FriendsManagementApp FriendsManagement { get; private set; } = 
            new FriendsManagementApp.FriendsManagementApp();
        public ChatApp.ChatApp Chat { get; private set; } = new ChatApp.ChatApp();

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return RoomList;
            yield return AppManager;
            yield return Friends;
            yield return FriendsManagement;
            yield return VoiceChat;
            yield return Chat;
        }
    }
}
