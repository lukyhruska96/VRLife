using System.Collections;
using VrLifeAPI.Client.Applications.DefaultApps.AppManager;
using VrLifeAPI.Client.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsManagementApp;
using VrLifeAPI.Client.Applications.DefaultApps.RoomListApp;
using VrLifeAPI.Client.Applications.DefaultApps.VoiceChatApp;

namespace VrLifeAPI.Client.Applications.DefaultApps
{
    public interface IDefaultApps : IEnumerable
    {
        IRoomListApp RoomList { get; }
        IAppManager AppManager { get; }
        IFriendsApp Friends { get; }
        IVoiceChatApp VoiceChat { get; }
        IFriendsManagementApp FriendsManagement { get; }
        IChatApp Chat { get; }
    }
}
